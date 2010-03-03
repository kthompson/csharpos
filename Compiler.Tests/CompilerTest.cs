using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using System.IO;
using System.Diagnostics;
using Mono.Cecil.Cil;

namespace Compiler.Tests
{
    public abstract class CompilerTest
    {
        protected string CompileAndRunMethod<TReturn>(Action<CilWorker> action)
        {
            return CompileAndRunMethod(GenerateMethod<TReturn>(action));
        }

        protected string CompileAndRunMethod<T>(Func<T> action)
        {
            return CompileAndRunMethod(GenerateMethod(action));
        }

        protected MethodDefinition GenerateMethod<TReturn>(Action<CilWorker> action)
        {
            var method = new MethodDefinition(Helper.GetRandomString("TestMethod"), MethodAttributes.Static | MethodAttributes.Public, GetCorlibType<TReturn>());
            action(method.Body.CilWorker);
            return method;
        }

        protected MethodDefinition GenerateMethod<T>(Func<T> action)
        {
            var method = this.Assembly.MainModule.Import(action.Method).Resolve();
            method.Name = Helper.GetRandomString("TestMethod");
            method.Body.Simplify();
            return method;
        }

        protected string CompileAndRunMethod(MethodDefinition method)
        {
            try
            {
                //create the runtime
                GenerateRuntime(method);

                //compile the method to ASM
                CompileMethod(method);

                //run gcc and compile the runtime and ASM together
                BuildTest();

                //run the compiled exe and return output
                return ExecuteTest();
            }
            finally
            {
                Cleanup();
            }
        }

        protected virtual void Cleanup()
        {
            Try.Each(
                        () => File.Delete("TempAssembly.dll"),
                        () => File.Delete("test.s"),
                        () => File.Delete("stack.s"),
                        () => File.Delete("runtime.c"),
                        () => File.Delete("test.exe")
                );
        }

  

        protected static TypeDefinition GetCorlibType<T>()
        {
            var resolver = new DefaultAssemblyResolver();
            var asm = resolver.Resolve("mscorlib");
            return asm.MainModule.Types[typeof(T).FullName];
        }

        private AssemblyDefinition _assembly;
        protected virtual AssemblyDefinition Assembly
        {
            get
            {
                if(_assembly == null)
                    _assembly = AssemblyFactory.DefineAssembly("TempAssembly", AssemblyKind.Dll);

                return _assembly;
            }
        }

        protected virtual void GenerateRuntime(TextWriter runtime, MethodDefinition method)
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
                    if(Debugger.IsAttached)
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

        private void GenerateRuntime(MethodDefinition method)
        {
            using (var runtime = new StreamWriter("runtime.c"))
            {
                GenerateRuntime(runtime, method);
            }

            using (var runtime = new StreamWriter("stack.s"))
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

        private static string ExecuteTest()
        {
            string output;
            string error;
            Helper.Execute("test.exe", out error, out output);
            return output;
        }

        private static void BuildTest()
        {
            string output;
            string error;
            if (Helper.Execute("gcc -Wall test.s stack.s runtime.c -o test.exe", out error, out output) == 0) 
                return;

            Helper.Stop(() => new BuildException(error, output));
        }

        private static void CompileMethod(MethodDefinition method)
        {
            using (var output = new StreamWriter("test.s"))
            {
                var compiler = new MethodCompilerStage();
                compiler.Run(new MethodCompilerContext(method, output));
            }
        }

       
    }
}
