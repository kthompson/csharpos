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
            var outputFile = string.Empty;
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
            if (inputs.Length == 0) 
                return;

            Console.WriteLine("Usage: compiler -o output.exe input.exe");
            Console.WriteLine(parser.GetUsage());


            //FIXME: make this use architecture etc
            //var assembly = AssemblyFactory.GetAssembly(inputs[0]);
            //var compiler = new AssemblyCompiler(new MethodCompilerStage(), new GccBuildStage(outputFile));
            //var context = new AssemblyCompilerContext(assembly, assembly.EntryPoint);
            //compiler.Compile(context);
        }
    }
}
