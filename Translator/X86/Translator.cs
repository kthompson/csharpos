using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Translator;

namespace Compiler.X86
{
    public class Translator : ITranslator
    {

        #region ITranslator Members

        public List<MethodReference> CollectMethodReferences(MethodDefinition method)
        {
            var list = new List<MethodReference>();
            foreach (Instruction instruction in method.Body.Instructions)
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


