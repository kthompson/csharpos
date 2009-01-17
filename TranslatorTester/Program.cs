using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Reflection;

namespace Translator
{
    class Program
    {
        static void Main(string[] args)
        {
			string appLocation = Assembly.GetExecutingAssembly().Location;
			AssemblyDefinition definition = AssemblyFactory.GetAssembly(appLocation);
			foreach(TypeDefinition type in definition.MainModule.Types)
			{
				foreach(MethodDefinition method in type.Methods)
					BuildMethod(method);
			}
        }

		static void BuildMethod(MethodDefinition method)
		{
			Console.WriteLine(method.Name);
			foreach (Instruction instruction in method.Body.Instructions) {
				Console.Write(string.Format("  0x{0,4:X}: {1} ",instruction.Offset,instruction.OpCode.ToString()));
				if(instruction.Operand != null)
				{
					Type operandType = instruction.Operand.GetType();
					
					if(operandType == typeof(MethodReference))
					{
						MethodReference reference = instruction.Operand as MethodReference;
						Console.Write(reference.DeclaringType.Name +"."+ reference.Name);
					}
					else if(operandType == typeof(MethodDefinition))
						Console.Write((instruction.Operand as MethodDefinition).Name);
					else if(operandType == typeof(Instruction))
						Console.Write(string.Format("0x{0,4:X}",(instruction.Operand as Instruction).Offset));
					else if(operandType == typeof(TypeReference))
						Console.Write((instruction.Operand as TypeReference).FullName);
					else if(operandType == typeof(VariableDefinition))
					{
						VariableDefinition variable = instruction.Operand as VariableDefinition;
						Console.Write(string.Format("{0}({1})",variable.Name, variable.VariableType.Name));
					}
					else
						Console.Write(string.Format(" {0} [{1}]",instruction.Operand.ToString(),instruction.Operand.GetType().FullName));
				}
				Console.WriteLine();
			} 
		}
    }
}
