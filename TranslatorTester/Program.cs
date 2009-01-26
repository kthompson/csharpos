using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Reflection;
using Kernel;
using Indy.IL2CPU;

namespace TranslatorTester
{
    class Program
    {
        static void Main(string[] args)
        {
            var engine = new Engine();
            var location = Assembly.GetExecutingAssembly().Location;
            engine.Execute(location, TargetPlatformEnum.X86, hi => "", new string[]{}, DebugMode.None, false, 0, ".\\", false);

            
        }

        public static void Init()
        {
            var location = Assembly.GetExecutingAssembly().Location;
            var definition = AssemblyFactory.GetAssembly(location);
            definition.MainModule.Accept(new CILReflectionPrinter());

            Console.Read();
        }
    }
}
