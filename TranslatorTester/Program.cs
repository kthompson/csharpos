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
            var engine = new Engine { DebugMode = DebugMode.None, OutputDirectory = @"C:\Program Files\Cosmos User Kit\Tools\asm\" };
            var location = Assembly.GetExecutingAssembly().Location;
            var plugs = new string[] {
                @"C:\Program Files\Cosmos User Kit\Tools\Cosmos.Kernel.Plugs\Cosmos.Kernel.Plugs.dll",
                @"C:\Program Files\Cosmos User Kit\Tools\Cosmos.Hardware.Plugs\Cosmos.Hardware.Plugs.dll",
                @"C:\Program Files\Cosmos User Kit\Tools\Cosmos.Sys.Plugs\Cosmos.Sys.Plugs.dll"
            };
            
            engine.Execute(location, null, plugs, false, false);
        }

        public static void Init()
        {
            //var location = Assembly.GetExecutingAssembly().Location;
            //var definition = AssemblyFactory.GetAssembly(location);
            //definition.MainModule.Accept(new CILReflectionPrinter());
            var engine = new VirtualExecutionSystem.Engine(typeof(Program).GetMethod("DebuggerTest"));
            engine.Start();

            Console.WriteLine("hello world");
        }

        public static void DebuggerTest()
        {
            var i = 1;
            var j = 2 + i;
            var k = 3 + j;
            var l = 4 + k;
            var m = 5 + l;
        }
    }
}
