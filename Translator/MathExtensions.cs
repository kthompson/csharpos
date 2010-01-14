using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Translator
{
    public static class MathExtensions
    {
        public static unsafe int ToIEEE754(this float value)
        {
            return *(((int*)&value));
        }
    }
}
