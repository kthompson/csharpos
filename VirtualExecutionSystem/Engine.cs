using System;
using System.Collections.Generic;
using System.Text;
using Mono.Cecil;
using System.Reflection;

namespace VirtualExecutionSystem
{
    public class Engine
    {
        public MethodDefinition EntryPoint { get; private set; }

        public Engine(AssemblyDefinition assembly)
        {
            this.EntryPoint = assembly.EntryPoint;
        }

        public Engine(MethodDefinition entryPoint)
        {
            this.EntryPoint = entryPoint;
        }

        public Engine(MethodInfo method)
        {
            Type type = method.DeclaringType;
            AssemblyDefinition asm = AssemblyFactory.GetAssembly(type.Assembly.Location);
            var finder = new MethodFinder(method);
            asm.Accept(finder);
            if (finder.Found)
                this.EntryPoint = finder.Method;
            else
                throw new ArgumentException("Could not find the method");
        }

        public void Start()
        {
            this.EntryPoint.Body.Accept(new MethodRunner());
        }
    }
}
