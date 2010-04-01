using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Compiler.Framework;
using Mono.Cecil;

namespace Compiler
{
    public class AssemblyCompilerContext : IAssemblyCompilerContext
    {
        public AssemblyDefinition AssemblyDefinition { get; private set; }
        public List<IMethodCompilerContext> MethodContexts { get; private set; }
        public List<OutputFile> OutputFiles { get; private set; }
        public string Output { get; private set; }

        public AssemblyCompilerContext(AssemblyDefinition assemblyDefinition, string output)
        {
            this.AssemblyDefinition = assemblyDefinition;
            this.MethodContexts = new List<IMethodCompilerContext>();
            this.OutputFiles = new List<OutputFile>();
            this.Output = output;
        }

        public TextWriter GetOutputFileWriter(string filename)
        {
            var output = new OutputFile(filename);
            this.OutputFiles.Add(output);
            return output.Out;
        }
    }
}
