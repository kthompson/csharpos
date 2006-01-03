//
// GenericParameter.cs
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

	using System;

	public sealed class GenericParameter : TypeReference, IGenericParameter {

		int m_position;
		string m_name;
		GenericParamAttributes m_attributes;
		IGenericParameterProvider m_owner;
		TypeReferenceCollection m_constraints;

		public int Position {
			get { return m_position; }
			set { m_position = value; }
		}

		public GenericParamAttributes Attributes {
			get { return m_attributes; }
			set { m_attributes = value; }
		}

		public IGenericParameterProvider Owner {
			get { return m_owner; }
		}

		public TypeReferenceCollection Constraints {
			get {
				if (m_constraints == null)
					m_constraints = new TypeReferenceCollection (null);

				return m_constraints;
			}
		}

		public override string Name {
			get {
				if (m_name != null)
					return m_name;

				if (m_owner is TypeDefinition)
					return string.Concat ("!", m_position.ToString ());
				else if (m_owner is MethodDefinition)
					return string.Concat ("!!", m_position.ToString ());
				else
					throw new InvalidOperationException ();
			}
			set { m_name = value; }
		}

		public override string Namespace {
			get { return string.Empty; }
			set { throw new InvalidOperationException (); }
		}

		public override string FullName {
			get { return Name; }
		}

		internal GenericParameter (int pos, IGenericParameterProvider owner) :
			base (string.Empty, string.Empty)
		{
			m_position = pos;
			m_owner = owner;
		}

		public GenericParameter (string name, IGenericParameterProvider owner) :
			base (string.Empty, string.Empty)
		{
			m_name = name;
			m_owner = owner;
		}
	}
}
