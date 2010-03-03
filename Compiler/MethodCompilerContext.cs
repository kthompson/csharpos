using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Mono.Cecil;

namespace Compiler
{
    public class MethodCompilerContext : ICompilerContext
    {
        public MethodReference Method { get; private set; }
        public TextWriter Out { get; private set; }

        public MethodCompilerContext(MethodReference method, TextWriter writer)
        {
            this.Method = method;
            this.Out = writer;
        }

        public MethodCompilerContext(MethodReference method)
            : this(method, new StringWriter())
        {
        }
    }
}
