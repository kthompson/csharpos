using System.Text;

namespace Compiler
{
    /// <summary>
    /// Run gcc to compile the runtime and ASM together
    /// </summary>
    public class GccBuildStage : BaseCompilerStage
    {
        public override string Name
        {
            get { return "Build Stage"; }
        }

        public string Filename { get; private set; }

        public GccBuildStage(string filename)
        {
            this.Filename = filename;  
        }

        public override ICompilerContext Run(ICompilerContext context)
        {
            var assemblyContext = context as AssemblyCompilerContext;
            if (assemblyContext == null)
                return context;

            var cmd = new StringBuilder("gcc -Wall -o ");
            cmd.Append(this.Filename);

            foreach (var file in assemblyContext.OutputFiles)
                cmd.AppendFormat(" {0}", file.Filename);

            string output;
            string error;
            if (Helper.Execute(cmd.ToString(), out error, out output) == 0)
                return context;

            Helper.Stop(() => new BuildException(error, output));

            return null;
        }
    }
}


