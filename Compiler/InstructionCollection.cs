using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Compiler
{
    public class InstructionCollection : Collection<IInstruction>
    {
        private LinkedList<IInstruction> _instructions;

        protected override void InsertItem(int index, IInstruction item)
        {
            _instructions.
            base.InsertItem(index, item);
            this.Items[]
        }

        protected override void SetItem(int index, IInstruction item)
        {
            base.SetItem(index, item);
        }

        protected override void RemoveItem(int index)
        {
            base.RemoveItem(index);
        }
    }
}
