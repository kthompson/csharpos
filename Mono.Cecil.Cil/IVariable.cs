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

namespace Mono.Cecil.Cil {

    using Mono.Cecil;

    public interface IVariableReference : ICodeVisitable {
        IMethodDefinition Method { get; set; }
        string Name { get; set; }
    }

    public interface IVariableDefinition : IVariableReference {
        ITypeReference Variable { get; set; }
    }
}
