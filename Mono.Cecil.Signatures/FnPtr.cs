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

namespace Mono.Cecil.Signatures {

    internal sealed class FnPtr : SigType {

        private MethodSig m_method;

        public MethodSig Method {
            get { return m_method; }
            set { m_method = value; }
        }

        public FnPtr () : base (ElementType.FnPtr)
        {
        }
    }
}
