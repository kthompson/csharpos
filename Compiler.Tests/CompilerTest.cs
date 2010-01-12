using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using System.IO;
using System.Diagnostics;
using Translator;

namespace Compiler.Tests
{
    public abstract class CompilerTest
    {
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
            File.Delete("TempAssembly.dll");
            File.Delete("test.s");
            File.Delete("runtime.c");
            File.Delete("test.exe");
        }

        protected TypeDefinition GenerateType(
            string name = null,
            string ns = "TestNamespace", 
            TypeAttributes attributes = TypeAttributes.Class | TypeAttributes.Public, 
            TypeReference baseType = null, 
            params Func<TypeDefinition, MethodDefinition>[] generateMethods)
        {
            if (name == null)
                name = RandomString("class");

            var type = new TypeDefinition(name, ns, attributes, baseType);
            foreach (var generateMethod in generateMethods)
                type.Methods.Add(generateMethod(type));

            this.Assembly.MainModule.Types.Add(type);
            return type;
        }

        protected static string RandomString(string prefix, int length = 32)
        {
            string tempString = prefix;
            
            while (tempString.Length < length)
                tempString += Guid.NewGuid().ToString().Replace("-", "").ToLower(); 

            return tempString.Substring(0, length); 
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
            runtime.WriteLine("#include <stdio.h>");
            runtime.WriteLine();
            runtime.WriteLine("int main(int argc, char** argv)");
            runtime.WriteLine("{");
            runtime.WriteLine(string.Format("	printf(\"%d\\n\", {0}());", method.Name));
            runtime.WriteLine("	return 0;");
            runtime.WriteLine("}");
        }

        private void GenerateRuntime(MethodDefinition method)
        {
            using (var runtime = new StreamWriter("runtime.c"))
            {
                GenerateRuntime(runtime, method);
            }
        }

        private static string ExecuteTest()
        {
            string output;
            string error;
            Execute("test.exe", out error, out output);
            return output;
        }

        private static void BuildTest()
        {
            string output;
            string error;
            if (Execute("gcc -Wall test.s runtime.c -o test.exe", out error, out output) == 0) 
                return;

            Assert.Break();
        }

        private static void CompileMethod(MethodDefinition method)
        {
            using (var output = new StreamWriter("test.s"))
            {
                var compiler = new MethodCompiler(method, new Emitter(output), null);
                compiler.Compile();
            }
        }

        private static int Execute(string command, out string error, out string output)
        {
            var parts = command.Split(' ');
            var cmd = parts.First();
            var args = string.Join(" ", parts.Skip(1));
            var proc = Process.Start(new ProcessStartInfo(FullCommandPath(cmd), args)
            {
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                UseShellExecute = false
            });

            var outputTmp = new StringBuilder();
            var errorTmp = new StringBuilder();

            proc.OutputDataReceived += (sender, e) => outputTmp.AppendLine(e.Data);
            proc.ErrorDataReceived += (sender, e) => errorTmp.AppendLine(e.Data);
            
            proc.BeginOutputReadLine();
            proc.BeginErrorReadLine();
            
            proc.WaitForExit();

            outputTmp.Remove(outputTmp.Length - 2, 2);
            errorTmp.Remove(errorTmp.Length - 2, 2);

            output = outputTmp.ToString();
            error = errorTmp.ToString();

            return proc.ExitCode;
        }

        private static string FullCommandPath(string command)
        {
            var path = Directory.GetCurrentDirectory() +";" + Environment.GetEnvironmentVariable("PATH");
            var paths = path.Split(';');

            foreach (var baseDir in paths)
            {
                string file = Path.Combine(baseDir, command);
                if (File.Exists(file))
                    return file;

                file = Path.Combine(baseDir, command + ".exe");
                if (File.Exists(file))
                    return file;
            }

            return command;
        }
    }
}
