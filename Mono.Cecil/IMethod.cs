/*
 * Copyright (c) 2004 DotNetGuru and the individuals listed
 * on the ChangeLog entries.
 *
 * Authors :
 *   Jb Evain   (jb.evain@dotnetguru.org)
 *
 * This is a free software distributed under a MIT/X11 license
 * See LICENSE.MIT file for more details
 *
 *****************************************************************************/

namespace Mono.Cecil {

    using Mono.Cecil.Cil;

    public interface IMethodReference : IMethodSignature, IMemberReference, IReflectionVisitable {
    }

    public interface IMethodDefinition : IMemberDefinition, IMethodReference, IHasSecurity {

        MethodAttributes Attributes { get; set; }
        MethodImplAttributes ImplAttributes { get; set; }
        MethodSemanticsAttributes SemanticsAttributes { get; set; }

        IOverrideCollection Overrides { get; }
        IMethodBody Body { get; }
        IPInvokeInfo PInvokeInfo { get; }
    }
}
