using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Translator
{
    public class MethodCompiler : IMethodCompiler
    {
        public IAssemblyCompiler AssemblyCompiler { get; private set; }

        private MethodDefinition _methodDefinition;
        public MethodDefinition MethodDefinition
        {
            get
            {
                if (_methodDefinition == null)
                    _methodDefinition = this.Method.Resolve();
                return _methodDefinition;
            }
        }

        #region IMethodCompiler Members

        public MethodReference Method { get; private set; }
        public IEmitter Emitter { get; private set; }

        #endregion

        public MethodCompiler(MethodReference method, IAssemblyCompiler ac)
            : this(method, new Emitter(), ac)
        {

        }

        public MethodCompiler(MethodReference method, IEmitter emitter, IAssemblyCompiler ac)
        {
            Assert.IsNotNull(method);
            Assert.IsNotNull(emitter);

            this.AssemblyCompiler = ac;
            this.Method = method;
            this.Emitter = emitter;
        }

        #region ICompiler Members

        public void Compile()
        {
            this.MethodDefinition.Body.Accept(this.Emitter);
        }

        #endregion

        protected void Emit(string asm)
        {
        }
    }
}
