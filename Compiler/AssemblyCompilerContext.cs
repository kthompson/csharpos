using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Mono.Cecil;

namespace Compiler
{
    public class AssemblyCompilerContext : ICompilerContext
    {
        public AssemblyDefinition AssemblyDefinition { get; private set; }
        public List<MethodDefinition> Methods { get; private set; }
        public List<OutputFile> OutputFiles { get; private set; }

        public AssemblyCompilerContext(AssemblyDefinition assemblyDefinition, params MethodDefinition[] methods)
        {
            this.AssemblyDefinition = assemblyDefinition;
            this.Methods = new List<MethodDefinition>(methods);
            this.OutputFiles = new List<OutputFile>();
        }

        public TextWriter GetOutputFileWriter(string filename)
        {
            var output = new OutputFile(filename);
            this.OutputFiles.Add(output);
            return output.Out;
        }
    }
}
