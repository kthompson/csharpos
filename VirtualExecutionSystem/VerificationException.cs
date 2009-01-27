using System;
using System.Collections.Generic;
using System.Text;

namespace VirtualExecutionSystem
{
    public class VerificationException : Exception
    {
        public VerificationException()
        {

        }

        public VerificationException(string message)
            : base(message)
        {

        }
    }
}
