using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Reflection;
using Kernel;
using System.IO;
using Mono.Cecil.Binary;
using VirtualExecutionSystem;

namespace TranslatorTester
{
    class Program
    {
        static void Main(string[] args)
        {
            AssemblyDefinition asm = AssemblyFactory.GetAssembly(Assembly.GetExecutingAssembly().Location);
            var method = asm.Modules.First().Types.First(type => type.Value.Name == "Program").Value.Methods.First(method => method.Name == "DebuggerTest");
            
            
        }

        public static int DebuggerTest()
        {
            return 7;
        }
    }
}
