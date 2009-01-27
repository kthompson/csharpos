using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Reflection;
using Kernel;
using Indy.IL2CPU;
using System.IO;

namespace TranslatorTester
{
    class Program
    {
        static void Main(string[] args)
        {
            var engine = new Engine();
            var location = Assembly.GetExecutingAssembly().Location;
            var asmPath = @"C:\Program Files\Cosmos User Kit\Tools\asm\";
            var plugs = new string[] {
                @"C:\Program Files\Cosmos User Kit\Tools\Cosmos.Kernel.Plugs\Cosmos.Kernel.Plugs.dll",
                @"C:\Program Files\Cosmos User Kit\Tools\Cosmos.Hardware.Plugs\Cosmos.Hardware.Plugs.dll",
                @"C:\Program Files\Cosmos User Kit\Tools\Cosmos.Sys.Plugs\Cosmos.Sys.Plugs.dll"
            };
            engine.CompilingMethods += new Action<int, int>(engine_CompilingMethods);
            engine.CompilingStaticFields += new Action<int, int>(engine_CompilingStaticFields);
            engine.Execute(location, TargetPlatformEnum.X86,  g => Path.Combine(asmPath, g + ".asm"), plugs, DebugMode.None, false, 0, asmPath, false);
        }

        static void engine_CompilingStaticFields(int arg1, int arg2)
        {

        }

        static void engine_CompilingMethods(int arg1, int arg2)
        {
                
        }

        public static void Init()
        {
            //var location = Assembly.GetExecutingAssembly().Location;
            //var definition = AssemblyFactory.GetAssembly(location);
            //definition.MainModule.Accept(new CILReflectionPrinter());
            Console.WriteLine("hello world");
        }
    }
}
