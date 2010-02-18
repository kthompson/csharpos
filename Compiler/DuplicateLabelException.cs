using System;

namespace Compiler
{
    public class DuplicateLabelException : Exception
    {
        public string LabelName { get; private set; }

        public DuplicateLabelException(string labelName)
        {
            this.LabelName = labelName;
        }
    }
}


