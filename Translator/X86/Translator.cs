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

        public List<MethodReference> CollectMethodReferences(MethodDefinition method)
        {
            var list = new List<MethodReference>();
            foreach (Mono.Cecil.Cil.Instruction instruction in method.Body.Instructions)
            {
                var reference = instruction.Operand as MethodReference;
                if (reference == null)
                    continue;

                //reference
            }
            return list;
        }

        #endregion
    }
}
