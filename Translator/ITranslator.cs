using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil.Cil;

namespace Translator
{
    public interface ITranslator
    {
        byte[] GetAssemblyBytes(Code opcode);
    }
}
