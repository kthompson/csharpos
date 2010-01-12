using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Mono.Cecil;
using System.IO;
using System.Diagnostics;
using Translator;

namespace Compiler.Tests
{
    public abstract class CompilerTest
    {
        private static string GetRuntime(string name)
        {
            var sb = new StringBuilder();
            sb.AppendLine("#include <stdio.h>");
            sb.AppendLine();
            sb.AppendLine("int main(int argc, char** argv)");
            sb.AppendLine("{");
            sb.AppendLine(string.Format("	printf(\"%d\\n\", {0}());", name));
            sb.AppendLine("	return 0;");
            sb.AppendLine("}");
            return sb.ToString();
        }

        protected static string CompileAndRunMethod(TypeDefinition type, string methodName)
        {
            var error = string.Empty;
            var output = string.Empty;

            //create the runtime
            using (var runtime = new StreamWriter("runtime.c"))
            {
                runtime.Write(GetRuntime(methodName));
            }

            //compile the method to ASM
            ILCompile(type, methodName);

            //run gcc and compile the runtime and ASM together
            //      gcc -Wall ctest.s runtime.c -o test            
            if (Execute("gcc -Wall " + methodName + ".s runtime.c -o test.exe", out error, out output) != 0)
            {
                Assert.Break();
                return null;
            }

            //run the compiled output
            //      test
            Execute("test.exe", out error, out output);

            //cleanup
            File.Delete(methodName + ".s");
            File.Delete("runtime.c");
            File.Delete("test.exe");
            //return output as a string
            return output;
        }

        private static void ILCompile(TypeDefinition type, string methodName)
        {
            var method = type.Methods.Find(m => m.Name == methodName);
            using (var output = new StreamWriter(methodName + ".s"))
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
            switch(command)
            {
                case "gcc":
                    return @"c:\MinGW\bin\gcc.exe";
                default:
                    var file = Path.Combine(@"C:\Documents and Settings\kthompson\code\csharpos\Compiler.Tests\bin", command);
                    if (File.Exists(file))
                        return file;
                    file += ".exe";
                    if (File.Exists(file))
                        return file;

                    return file;
            }
        }
    }
}
