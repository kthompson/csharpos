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
            //if (HasFlag(method, Mono.Cecil.MethodAttributes.Assem)) throw new NotImplementedException();
            //if (HasFlag(method, Mono.Cecil.MethodAttributes.Compilercontrolled)) throw new NotImplementedException();
            //if (HasFlag(method, Mono.Cecil.MethodAttributes.FamANDAssem)) throw new NotImplementedException();
            //if (HasFlag(method, Mono.Cecil.MethodAttributes.FamORAssem)) throw new NotImplementedException();
            //if (HasFlag(method, Mono.Cecil.MethodAttributes.Family)) throw new NotImplementedException();
            //if (HasFlag(method, Mono.Cecil.MethodAttributes.Final)) throw new NotImplementedException();
            //if (HasFlag(method, Mono.Cecil.MethodAttributes.HasSecurity)) throw new NotImplementedException();
            if (HasFlag(method, Mono.Cecil.MethodAttributes.Private)) Console.Write("private ");
            if (HasFlag(method, Mono.Cecil.MethodAttributes.Public)) Console.Write("public ");
            if (HasFlag(method, Mono.Cecil.MethodAttributes.HideBySig)) Console.Write("hidebysig ");
            //if (HasFlag(method, Mono.Cecil.MethodAttributes.MemberAccessMask)) throw new NotImplementedException();
            //if (HasFlag(method, Mono.Cecil.MethodAttributes.NewSlot)) throw new NotImplementedException();
            //if (HasFlag(method, Mono.Cecil.MethodAttributes.PInvokeImpl)) throw new NotImplementedException();
            //if (HasFlag(method, Mono.Cecil.MethodAttributes.RTSpecialName)) throw new NotImplementedException();
            //if (HasFlag(method, Mono.Cecil.MethodAttributes.RequireSecObject)) throw new NotImplementedException();
            //if (HasFlag(method, Mono.Cecil.MethodAttributes.ReuseSlot)) throw new NotImplementedException();
            //if (HasFlag(method, Mono.Cecil.MethodAttributes.SpecialName)) throw new NotImplementedException();
            if (HasFlag(method, Mono.Cecil.MethodAttributes.Static)) Console.Write("static ");
            //if (HasFlag(method, Mono.Cecil.MethodAttributes.UnmanagedExport)) throw new NotImplementedException();
            //if (HasFlag(method, Mono.Cecil.MethodAttributes.Virtual)) throw new NotImplementedException();
            //if (HasFlag(method, Mono.Cecil.MethodAttributes.VtableLayoutMask)) throw new NotImplementedException();

            Console.Write(method.ReturnType.ReturnType.Name + " ");
            Console.Write(method.Name);

            Console.Write("(");
            for (var i = 0; i < method.Parameters.Count; i++)
            {
                if (i > 0) Console.Write(", ");
                var param = method.Parameters[i];
                VisitParameterDefinition(param);               
            }
            Console.Write(")");
            if (method.IsIL) Console.Write(" cil");
            if (method.IsManaged) Console.Write(" managed");
            Console.WriteLine();
            Console.WriteLine("{");

            foreach (Instruction instruction in method.Body.Instructions)
            {
                Console.Write(string.Format("  L_{0}: {1} ", instruction.Offset.ToString("x").PadLeft(4, '0'), instruction.OpCode.ToString()));
                if (instruction.Operand != null)
                {
                    Type operandType = instruction.Operand.GetType();

                    if (operandType == typeof(MethodReference))
                    {
                        MethodReference reference = instruction.Operand as MethodReference;
                        VisitTypeReference(reference.ReturnType.ReturnType);
                        Console.Write(" ");
                        VisitTypeReference(reference.DeclaringType);
                        Console.Write("::"+ reference.Name);
                    }
                    else if (operandType == typeof(MethodDefinition))
                        Console.Write((instruction.Operand as MethodDefinition).Name);
                    else if (operandType == typeof(Instruction))
                        Console.Write(string.Format("L_{0}", (instruction.Operand as Instruction).Offset.ToString("x").PadLeft(4, '0')));
                    else if (operandType == typeof(TypeReference))
                        VisitTypeReference(instruction.Operand as TypeReference);
                    else if (operandType == typeof(VariableDefinition))
                    {
                        VariableDefinition variable = instruction.Operand as VariableDefinition;
                        Console.Write(string.Format("{0}({1})", variable.Name, variable.VariableType.Name));
                    }
                    else
                        Console.Write(string.Format(" {0} [{1}]", instruction.Operand.ToString(), instruction.Operand.GetType().FullName));
                }
                Console.WriteLine();
            }
            Console.WriteLine("}");
        }

        

        public override void VisitParameterDefinition(ParameterDefinition parameter)
        {
            VisitTypeReference(parameter.ParameterType);
            Console.Write(" " + parameter.Name);
        }

        public override void VisitTypeReference(TypeReference type)
        {
            Console.Write("[{0}]{1}", type.Scope.Name, type.FullName);
        }

        private static bool HasFlag(MethodDefinition method, Mono.Cecil.MethodAttributes attribute)
        {
            return (method.Attributes & attribute) == attribute;
        }
    }
}
