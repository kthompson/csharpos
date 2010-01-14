using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Translator
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
