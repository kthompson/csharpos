using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Compiler.Framework;
using Envelop;
using Mono.Cecil;
using System.IO;
using System.Diagnostics;
using Mono.Cecil.Cil;

namespace Compiler.Tests
{
    public abstract class CompilerTest
    {
        protected readonly IKernel Builder;

        protected CompilerTest()
        {
            this.Builder = Kernel.Create();
            
            this.Builder.Load(new x86.X86Module());

            this.Builder.Bind<IMethodCompiler>().To<MethodCompiler>();
            this.Builder.Bind<IMethodCompilerStage>().To<MethodToAsmStage>();

            this.Builder.Bind<IAssemblyCompiler>().To<AssemblyCompiler>();

            this.Builder.Bind<IAssemblyCompilerStage>().To<GccBuildStage>();
            this.Builder.Bind<IAssemblyCompilerStage>().To<TestRuntimeStage>();
            this.Builder.Bind<IAssemblyCompilerStage>().To<MethodCompilerStage>();
            this.Builder.Bind<IAssemblyCompilerStage>().To<MethodQueuingStage>();
        }

        protected string CompileAndRunMethod<T>(Func<T> action)
        {
            return CompileAndRunMethod<Func<T>, object>(action);
        }

        protected string CompileAndRunMethod<TDelegate, TParam>(TDelegate action, params TParam[] arguments)
        {
            var asm = GetAssembly();
            var method = ImportMethod(asm, action);

            var context = new TestContext(asm, arguments.Cast<object>().ToArray());
            context.MethodContexts.Add(CodeStream.Create(context, method));

            try
            {
                //compile 
                context = this.AssemblyCompiler.Compile(context) as TestContext;
                Helper.IsNotNull(context);

                //run the compiled exe and return output
                return Helper.Execute(context.Output);
            }
            catch (Exception e)
            {
                Helper.Break();
                throw;
            }
            finally
            {
                if (context != null)
                {
                    foreach (var file in context.OutputFiles)
                        File.Delete(file.Filename);

                    File.Delete(context.Output);
                }
            }
        }

        private MethodDefinition ImportMethod<T>(AssemblyDefinition asm, T action)
        {
            var delegateAction = action as Delegate;
            if (delegateAction == null)
                throw new InvalidOperationException("action must be a delegate type");
            var method = asm.MainModule.Import(delegateAction.Method).Resolve();
            method.Name = Helper.GetRandomString("TestMethod");
            return method;
        }

        private AssemblyDefinition GetAssembly()
        {
            return AssemblyFactory.DefineAssembly("TempAssembly", AssemblyKind.Dll);
        }

        private IAssemblyCompiler _assemblyCompiler;
        private IAssemblyCompiler AssemblyCompiler
        {
            get
            {
                if (_assemblyCompiler == null)
                    _assemblyCompiler = this.Builder.Resolve<IAssemblyCompiler>();

                return _assemblyCompiler;
            }
        }
    }
}
