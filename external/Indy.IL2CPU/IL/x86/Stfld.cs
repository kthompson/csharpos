using System;
using System.Collections;
using System.Collections.Generic;		 
using System.Linq;


using CPU = Indy.IL2CPU.Assembler.X86;
using System.Reflection;
using Mono.Cecil;

namespace Indy.IL2CPU.IL.X86
{
    [OpCode(Mono.Cecil.Cil.Code.Stfld)]
    public class Stfld : Op
    {
        private readonly TypeInformation.Field mField;
        private readonly TypeInformation mType;
        public static void ScanOp(Mono.Cecil.Cil.Instruction instruction, MethodInformation aMethodInfo, SortedList<string, object> aMethodData)
        {
            var xField = instruction.Operand as FieldDefinition;
            if (xField == null)
            {
                throw new Exception("Field not found!");
            }
            Engine.RegisterType(xField.FieldType);
        }
        public Stfld(Mono.Cecil.Cil.Instruction instruction, MethodInformation aMethodInfo)
            : base(instruction, aMethodInfo)
        {
            if (instruction == null)
            {
                throw new ArgumentNullException("instruction");
            }
            if (aMethodInfo == null)
            {
                throw new ArgumentNullException("aMethodInfo");
            }
            var field = instruction.Operand as FieldReference;
            if (field == null)
            {
                throw new Exception("Field not found!");
            }
            string xFieldId = field.Name;
            mType = Engine.GetTypeInfo(field.DeclaringType);
            mField = mType.Fields[xFieldId];
        }

        public override void DoAssemble()
        {
            Stfld(Assembler, mType, mField);
        }
    }
}