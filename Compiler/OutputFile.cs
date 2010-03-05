using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Compiler
{
    public class OutputFile
    {
        public string Filename { get; private set; }
        public TextWriter Out { get; private set; }

        public OutputFile(string filename)
        {
            this.Filename = filename;
            this.Out = new StreamWriter(filename);
        }

        public override string ToString()
        {
            return this.Filename;
        }
    }
}
