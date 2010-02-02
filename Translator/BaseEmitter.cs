using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Compiler
{
    public class BaseEmitter : IEmitter 
    {
        #region IEmitter Members

        public virtual Section Section(SectionType type)
        {
            throw new NotImplementedException();
        }

        public virtual void Emit(MethodDefinition methodDefinition)
        {
        }

        public virtual void Emit(MethodBody methodBody)
        {
        }

        #endregion
    }
}
