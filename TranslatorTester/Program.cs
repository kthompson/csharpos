using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Reflection;

namespace TranslatorTester
{
    class Program
    {
        static void Main(string[] args)
        {
			string appLocation = Assembly.GetExecutingAssembly().Location;
			AssemblyDefinition definition = AssemblyFactory.GetAssembly(appLocation);
            definition.MainModule.Accept(new CILReflectionPrinter());

            Console.Read();
        }
    }
}
