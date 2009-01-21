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
			var location = Assembly.GetExecutingAssembly().Location;
			var definition = AssemblyFactory.GetAssembly(location);
            definition.MainModule.Accept(new CILReflectionPrinter());

            Console.Read();
        }
    }
}
