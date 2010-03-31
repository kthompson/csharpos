using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using Compiler.Framework;

namespace Compiler.x86
{
    public class X86Module : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => new Architecture()).As<IArchitecture>();
            base.Load(builder);
        }
    }
}
