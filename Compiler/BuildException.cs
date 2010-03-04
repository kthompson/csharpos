using System;

namespace Compiler
{
    public class BuildException : ApplicationException
    {
        public string ErrorLog { get; private set; }
        public string OutputLog { get; private set; }

        public BuildException(string errorLog, string outputLog)
            : base(errorLog)
        {
            this.ErrorLog = errorLog;
            this.OutputLog = outputLog;
        }
    }
}


