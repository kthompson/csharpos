using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Compiler.Framework;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Compiler
{
    public class CodeStream : IMethodCompilerContext, IEnumerable<IInstruction>
    {
        public IInstruction Outside { get; private set; }
        public MethodDefinition Method { get; private set; }
        public MethodBody Body { get; private set; }

        private CodeStream(IAssemblyCompilerContext assemblyCompilerContext)
        {
            this.AssemblyCompilerContext = assemblyCompilerContext;
        }

        public static CodeStream Create(IAssemblyCompilerContext assemblyCompilerContext, MethodReference method)
        {
            var m = method.Resolve();
            var body = m.Body;
            var stream = new CodeStream(assemblyCompilerContext)
             {
                 Method = m,
                 Body = body,
                 Outside = new CilInstruction(body.Instructions.Outside)
             };

            var il = stream.Body.Instructions.Outside;
            var ilCopy = stream.Outside;
            while (il.Next != null)
            {
                ilCopy.Next = new CilInstruction(il.Next) { Previous = ilCopy };
                ilCopy = ilCopy.Next;
                il = il.Next;
            }

            return stream;
        }

        #region IEnumerable<IInstruction> Members

        public IEnumerator<IInstruction> GetEnumerator()
        {
            var value = this.Outside;
            while ((value = value.Next) != null)
                yield return value;
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion

        #region IMethodCompilerContext Members

        private int _labelCount;
        public Label GetUniqueLabel()
        {
            return new Label(string.Format("L_{0}", _labelCount++));
        }

        public IAssemblyCompilerContext AssemblyCompilerContext { get; private set; }

        #endregion
    }
}
