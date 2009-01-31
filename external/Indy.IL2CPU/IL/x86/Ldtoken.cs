using System;
using System.Collections.Generic;
using System.IO;
using Indy.IL2CPU.Assembler;


using CPU = Indy.IL2CPU.Assembler.X86;
using System.Reflection;
using Mono.Cecil;

namespace Indy.IL2CPU.IL.X86
{
    [OpCode(OpCodeEnum.Ldtoken)]
    public class Ldtoken : Op
    {
        private string mTokenAddress;

        public static void ScanOp(Mono.Cecil.Cil.Instruction instruction, MethodInformation aMethodInfo, SortedList<string, object> aMethodData)
        {
            var xFieldDef = instruction.Operand as FieldDefinition;
            if (xFieldDef != null)
            {
                if (!xFieldDef.IsStatic)
                {
                    throw new Exception("Nonstatic field-backed tokens not supported yet!");
                }
                Engine.QueueStaticField(xFieldDef);
                return;
            }
            var typeRef = instruction.Operand;
            if (typeRef != null)
            {
                return;
            }
            throw new Exception("Token type not supported yet!");
        }
        public Ldtoken(Mono.Cecil.Cil.Instruction instruction, MethodInformation aMethodInfo)
            : base(instruction, aMethodInfo)
        {
            // todo: add support for type tokens and method tokens
            var field = instruction.Operand as FieldReference;
            var fd = field.Resolve();
            if (fd != null)
            {
                if (!fd.IsStatic)
                {
                    throw new Exception("Nonstatic field-backed tokens not supported yet!");
                }
                Engine.QueueStaticField(fd);
                mTokenAddress = DataMember.GetStaticFieldName(fd);
                return;
            }
            var typeRef = instruction.Operand;
            if (typeRef != null)
            {
                throw new Exception("Type Tokens not supported atm!");
                //				mTokenAddress = "0" + Engine.RegisterType(xTypeRef).ToString("X") + "h";
                //return;
            }
            throw new Exception("Token type not supported yet!");
        }

        public override void DoAssemble()
        {
            new CPU.Push { DestinationRef = new ElementReference(mTokenAddress) };
            Assembler.StackContents.Push(new StackContent(4, typeof(uint)));
        }
    }
}