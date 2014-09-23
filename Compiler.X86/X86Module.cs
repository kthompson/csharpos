using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Compiler.Framework;
using Envelop;

namespace Compiler.x86
{
    public class X86Module : Module
    {
        protected override void Load()
        {
            this.Bind<IArchitecture>().To<Architecture>();
        }
    }
}
