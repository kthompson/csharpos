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
        protected string CompileAndRunMethod<T>(Func<T> action)
        {
            var asm = GetAssembly();
            var method = ImportMethod(asm, action);
            var context = new AssemblyCompilerContext(asm, method);
            
            try
            {
                //compile 
                this.Compiler.Compile(context);

                //run the compiled exe and return output
                return Helper.Execute(GetTestExecutable());
            }
            finally
            {
                foreach (var file in context.OutputFiles)
                    File.Delete(file.Filename);

                File.Delete(GetTestExecutable());
            }
        }

        private string GetTestExecutable()
        {
            return this.Compiler.Stages.OfType<GccBuildStage>().First().Filename;
        }

        private MethodDefinition ImportMethod<T>(AssemblyDefinition asm, Func<T> action)
        {
            var method = asm.MainModule.Import(action.Method).Resolve();
            method.Name = Helper.GetRandomString("TestMethod");
            return method;
        }

        protected virtual ICompilerStage GetRuntimeStage()
        {
            return new TestRuntimeStage();
        }

        private AssemblyDefinition GetAssembly()
        {
            return AssemblyFactory.DefineAssembly("TempAssembly", AssemblyKind.Dll);
        }

        private ICompiler _compiler;
        private ICompiler Compiler
        {
            get
            {
                if(_compiler == null)
                    _compiler = new AssemblyCompiler(GetRuntimeStage(), new MethodCompilerStage(), new GccBuildStage("test.exe"));
                return _compiler;
            }
        }
    }
}
