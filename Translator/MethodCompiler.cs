using System.IO;
using Mono.Cecil;
using Cecil.Decompiler;

namespace Compiler
{
    public class MethodCompiler
    {
        public IAssemblyCompiler AssemblyCompiler { get; private set; }

        private MethodDefinition _methodDefinition;
        public MethodDefinition MethodDefinition
        {
            get { return _methodDefinition ?? (_methodDefinition = this.Method.Resolve()); }
        }

        #region IMethodCompiler Members

        public MethodReference Method { get; private set; }
        public Emitter Emitter { get; private set; }

        #endregion

        public MethodCompiler(MethodReference method, IAssemblyCompiler ac, TextWriter writer = null)
        {
            Helper.IsNotNull(method);

            this.AssemblyCompiler = ac;
            this.Method = method;
            this.Emitter = new Emitter(writer ?? new StringWriter());
        }

        #region ICompiler Members

        public void Compile()
        {
            this.Emitter.VisitMethodDefinition(this.MethodDefinition);
        }

        #endregion
    }
}


