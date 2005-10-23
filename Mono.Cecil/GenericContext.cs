//
// GenericContext.cs
//
// Author:
//   Jb Evain (jbevain@gmail.com)
//
// (C) 2005 Jb Evain
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

namespace Mono.Cecil {

	internal class GenericContext {

		TypeDefinition m_type;
		MethodDefinition m_method;

		public TypeDefinition Type {
			get { return m_type; }
			set { m_type = value; }
		}

		public MethodDefinition Method {
			get { return m_method; }
			set { m_method = value; }
		}

		public GenericContext ()
		{
		}

		public GenericContext (TypeDefinition type)
		{
			m_type = type;
		}

		public GenericContext (MethodDefinition meth)
		{
			m_type = (TypeDefinition) meth.DeclaringType;
			m_method = meth;
		}

		public GenericContext (TypeDefinition type, MethodDefinition meth)
		{
			m_type = type;
			m_method = meth;
		}

		public GenericContext (IGenericParameterProvider provider)
		{
			if (provider is TypeDefinition)
				m_type = provider as TypeDefinition;
			else {
				m_method = provider as MethodDefinition;
				m_type = m_method.DeclaringType as TypeDefinition;
			}
		}

		public GenericContext Clone ()
		{
			return new GenericContext (m_type, m_method);
		}
	}
}
