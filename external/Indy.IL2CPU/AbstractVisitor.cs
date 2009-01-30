using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using System.Collections;
using Mono.Cecil.Cil;

namespace Indy.IL2CPU
{
    public abstract class AbstractVisitor<V>
        where V : AbstractVisitor<V>
    {

        protected void Run<T>(Action<V, T> action, T item)
        {
            if (action != null)
                action((V)this, item);
        }

        protected void Run<T>(Action<V, T> action, IEnumerable collection)
        {
            foreach (T item in collection)
                Run(action, item);
        }

    }
}
