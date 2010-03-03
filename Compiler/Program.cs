using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;

namespace Compiler
{
    class Program
    {
        static void Main(string[] args)
        {
            string outputFile;
            var parser = new OptionParser
                             {
                                 new Option
                                 {
                                     ShortForm = "o",
                                     LongForm = "output",
                                     Required = true,
                                     ActionWithParam = option => outputFile = option
                                 },
                             };
            var inputs = parser.Parse(args);
            if (inputs.Length != 1)
            {
                Console.WriteLine("Usage: compiler -o output.exe input.exe");
                Console.WriteLine(parser.GetUsage());
                return;
            }

            var assembly = AssemblyFactory.GetAssembly(inputs[0]);
            var methodContext = new MethodCompilerContext(assembly.EntryPoint);
            var compiler = new AssemblyCompiler(new MethodCompilerStage());
            compiler.Compile(methodContext);

        }
    }
}
