using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Translator.X86
{
    public class Translator : ITranslator
    {

        #region ITranslator Members

        public IMethod TranslateMethod(MethodDefinition method)
        {
			throw new NotImplementedException();
        }

        #endregion
    }
}
