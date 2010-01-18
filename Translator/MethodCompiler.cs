using Compiler.X86;
using Mono.Cecil;

namespace Compiler
{
    public class MethodCompiler : IMethodCompiler
    {
        public IAssemblyCompiler AssemblyCompiler { get; private set; }

        private MethodDefinition _methodDefinition;
        public MethodDefinition MethodDefinition
        {
            get { return _methodDefinition ?? (_methodDefinition = this.Method.Resolve()); }
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
            Helper.IsNotNull(method);
            Helper.IsNotNull(emitter);

            this.AssemblyCompiler = ac;
            this.Method = method;
            this.Emitter = emitter;
        }

        #region ICompiler Members

        public void Compile()
        {
            this.MethodDefinition.Body.Accept(new CilToX86CodeVisitor());
            this.MethodDefinition.Body.Accept(this.Emitter);
        }

        #endregion
    }
}


