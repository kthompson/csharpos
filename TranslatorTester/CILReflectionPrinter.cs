using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace TranslatorTester
{
    public class CILReflectionPrinter : BaseReflectionVisitor
    {
        public static Dictionary<string,string> Replacements { get; private set; }
        static CILReflectionPrinter()
        {
            Replacements = new Dictionary<string, string> {
                {typeof(string).FullName, "string"},
                {typeof(void).FullName, "void"},
            };
        }

        public MethodDefinition LastMethod { get; private set; }

        public override void VisitModuleDefinition(ModuleDefinition module)
        {
            foreach (TypeDefinition type in module.Types)
                type.Accept(this);
        }

        public override void VisitMethodDefinitionCollection(MethodDefinitionCollection methods)
        {
            foreach (MethodDefinition method in methods)
                method.Accept(this);
        }

        public override void VisitMethodDefinition(MethodDefinition method)
        {
            Console.Write(".method ");

            if (HasFlag(method, Mono.Cecil.MethodAttributes.Abstract)) Console.Write("abstract ");
            if (HasFlag(method, Mono.Cecil.MethodAttributes.Assem)) Console.Write("assembly ");
            //if (HasFlag(method, Mono.Cecil.MethodAttributes.Compilercontrolled)) throw new NotImplementedException();
            //if (HasFlag(method, Mono.Cecil.MethodAttributes.FamANDAssem)) throw new NotImplementedException();
            //if (HasFlag(method, Mono.Cecil.MethodAttributes.FamORAssem)) throw new NotImplementedException();
            //if (HasFlag(method, Mono.Cecil.MethodAttributes.Family)) throw new NotImplementedException();
            
            //if (HasFlag(method, Mono.Cecil.MethodAttributes.HasSecurity)) throw new NotImplementedException();
            if (HasFlag(method, Mono.Cecil.MethodAttributes.Private)) Console.Write("private ");
            if (HasFlag(method, Mono.Cecil.MethodAttributes.Public)) Console.Write("public ");
            if (HasFlag(method, Mono.Cecil.MethodAttributes.HideBySig)) Console.Write("hidebysig ");
            //if (HasFlag(method, Mono.Cecil.MethodAttributes.MemberAccessMask)) throw new NotImplementedException();
            if (HasFlag(method, Mono.Cecil.MethodAttributes.NewSlot)) Console.Write("newslot ");
            //if (HasFlag(method, Mono.Cecil.MethodAttributes.PInvokeImpl)) throw new NotImplementedException();
            if (HasFlag(method, Mono.Cecil.MethodAttributes.RTSpecialName)) Console.Write("rtspecialname ");
            //if (HasFlag(method, Mono.Cecil.MethodAttributes.RequireSecObject)) throw new NotImplementedException();
            //if (HasFlag(method, Mono.Cecil.MethodAttributes.ReuseSlot)) throw new NotImplementedException();
            if (HasFlag(method, Mono.Cecil.MethodAttributes.SpecialName)) Console.Write("specialname ");
            Console.Write(HasFlag(method, Mono.Cecil.MethodAttributes.Static) ? "static " : "instance ");
            //if (HasFlag(method, Mono.Cecil.MethodAttributes.UnmanagedExport)) throw new NotImplementedException();
            if (HasFlag(method, Mono.Cecil.MethodAttributes.Virtual)) Console.Write("virtual ");
            if (HasFlag(method, Mono.Cecil.MethodAttributes.Final)) Console.Write("final ");
            //if (HasFlag(method, Mono.Cecil.MethodAttributes.VtableLayoutMask)) throw new NotImplementedException();

            VisitTypeReference(method.ReturnType.ReturnType);
            Console.Write(" ");
            Console.Write(method.Name);

            Console.Write("(");
            VisitParameterDefinitionCollection(method.Parameters);
            Console.Write(")");
            if (method.IsIL) Console.Write(" cil");
            if (method.IsManaged) Console.Write(" managed");
            if (method.IsInternalCall) Console.Write(" internalcall");
            Console.WriteLine();
            Console.WriteLine("{");

            foreach (Instruction instruction in method.Body.Instructions)
            {
                Console.Write("  L_{0}: {1} ", instruction.Offset.ToString("x").PadLeft(4, '0'), instruction.OpCode.ToString());
                if (instruction.Operand != null)
                {
                    Type operandType = instruction.Operand.GetType();

                    if (typeof(MethodReference).IsAssignableFrom(operandType))
                        VisitMemberReference(instruction.Operand as MethodReference);
                    else if (typeof(TypeReference).IsAssignableFrom(operandType))
                        VisitTypeReference(instruction.Operand as TypeReference);
                    else if(typeof(MemberReference).IsAssignableFrom(operandType))
                        VisitMemberReference(instruction.Operand as MemberReference);
                    else if (typeof(Instruction).IsAssignableFrom(operandType))
                        Console.Write("L_{0}", (instruction.Operand as Instruction).Offset.ToString("x").PadLeft(4, '0'));
                    else if (typeof(VariableReference).IsAssignableFrom(operandType))
                    {
                        var variable = instruction.Operand as VariableDefinition;
                        Console.Write("{0}({1})", variable.Name, variable.VariableType.Name);
                    }
                    else if(operandType == typeof(string))
                        Console.Write("\"{0}\"", instruction.Operand.ToString().Replace("\"","\\\""), instruction.Operand.GetType().FullName);
                    else
                        Console.Write(" {0} [{1}]", instruction.Operand.ToString(), instruction.Operand.GetType().FullName);
                }
                Console.WriteLine();
            }
            Console.WriteLine("}");
            Console.WriteLine();
            this.LastMethod = method;
        }

        public override void VisitMemberReference(MemberReference member)
        {
            Console.Write(member.ToString());
        }

        public override void VisitParameterDefinitionCollection(ParameterDefinitionCollection parameters)
        {
            if (parameters.Count == 0 || this.LastMethod == parameters[0].Method)
                return;

            for (var i = 0; i < parameters.Count; i++)
            {
                if (i > 0) Console.Write(", ");
                VisitParameterDefinition(parameters[i]);
            }
        }

        public override void VisitParameterDefinition(ParameterDefinition parameter)
        {
            VisitTypeReference(parameter.ParameterType);
            Console.Write(" " + parameter.Name);
        }

        public override void VisitTypeReference(TypeReference type)
        {
            if (Replacements.ContainsKey(type.FullName))
                Console.Write(Replacements[type.FullName]);
            else if(type.Scope == null)
                Console.Write("{0}", type.FullName);
            else
                Console.Write("[{0}]{1}", type.Scope.Name, type.FullName);
        }

        private static bool HasFlag(MethodDefinition method, Mono.Cecil.MethodAttributes attribute)
        {
            return (method.Attributes & attribute) == attribute;
        }
    }
}
