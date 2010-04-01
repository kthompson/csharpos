using System.Text;
using Compiler.Framework;

namespace Compiler
{
    /// <summary>
    /// Run gcc to compile the runtime and ASM together
    /// </summary>
    public class GccBuildStage : CompilerStageBase
    {
        public override string Name
        {
            get { return "Build Stage"; }
        }

        public override IAssemblyCompilerContext Run(IAssemblyCompiler compiler, IAssemblyCompilerContext context)
        {
            var assemblyContext = context as AssemblyCompilerContext;
            if (assemblyContext == null)
                return context;

            var cmd = new StringBuilder("gcc -Wall -o ");
            cmd.Append(assemblyContext.Output);

            foreach (var file in assemblyContext.OutputFiles)
                cmd.AppendFormat(" {0}", file.Filename);

            string output;
            string error;
            if (Helper.Execute(cmd.ToString(), out error, out output) == 0)
            {
                if (!string.IsNullOrEmpty(error))
                    Helper.Break();

                return context;
            }

            Helper.Stop(() => new BuildException(error, output));

            return null;
        }
    }
}


