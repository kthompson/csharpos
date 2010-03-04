﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Mono.Cecil;

namespace Compiler.Tests
{
    class TestRuntimeStage : BaseCompilerStage
    {
        public override string Name
        {
            get { return "Test Runtime Generator"; }
        }

        public override ICompilerContext Run(ICompilerContext context)
        {
            var assemblyContext = context as AssemblyCompilerContext;
            if (assemblyContext == null)
                return context;

            var method = assemblyContext.Methods.First();

            CreateRuntime(assemblyContext, method);

            CreateStack(assemblyContext, method);

            return context;
        }

        private void CreateRuntime(AssemblyCompilerContext assemblyContext, MethodDefinition method)
        {
            using (var runtime = assemblyContext.GetOutputFileWriter("runtime.c"))
            {
                string printf;
                string function = "setup_stack(stack_base)";
                string returnType;
                switch (method.ReturnType.ReturnType.Name.ToLower())
                {
                    case "single":
                        printf = "%.3f";
                        returnType = "float";
                        break;
                    case "int32":
                        printf = "%d";
                        returnType = "long";
                        break;
                    case "boolean":
                        printf = "%s";
                        returnType = "bool";
                        function += " ? \"True\" : \"False\"";
                        break;
                    case "char":
                        printf = "%c";
                        returnType = "char";
                        break;
                    default:
                        printf = "%d";
                        returnType = "long";
                        if (Debugger.IsAttached)
                            Debugger.Break();
                        break;
                }

                runtime.WriteLine("#include <stdio.h>");
                runtime.WriteLine("#include <stdbool.h>");
                runtime.WriteLine("#include <stdlib.h>");
                runtime.WriteLine();
                runtime.WriteLine(string.Format("{0} setup_stack(char*);", returnType));
                runtime.WriteLine();
                runtime.WriteLine("int main(int argc, char** argv)");
                runtime.WriteLine("{");
                runtime.WriteLine("	int stack_size = (16 * 4096);");
                runtime.WriteLine("	char* stack_top = malloc(stack_size);");
                runtime.WriteLine("	char* stack_base = stack_top + stack_size;");
                runtime.WriteLine(string.Format("	printf(\"{0}\\n\", {1});", printf, function));
                runtime.WriteLine("	free(stack_top);");
                runtime.WriteLine("	return 0;");
                runtime.WriteLine("}");
            }
        }

        private void CreateStack(AssemblyCompilerContext assemblyContext, MethodDefinition method)
        {
            using (var runtime = assemblyContext.GetOutputFileWriter("stack.s"))
            {
                runtime.WriteLine(".globl _setup_stack");
                runtime.WriteLine("	.def	_setup_stack;	.scl	2;	.type	32;	.endef");
                runtime.WriteLine("_setup_stack:");
                runtime.WriteLine("	movl %esp, %ecx");
                runtime.WriteLine("	movl 4(%esp), %esp");
                runtime.WriteLine("	call _{0}", method.Name);
                runtime.WriteLine("	movl %ecx, %esp");
                runtime.WriteLine("	ret");
            }
        }
    }
}