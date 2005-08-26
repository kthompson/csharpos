/*
 * Copyright (c) 2004, 2005 DotNetGuru and the individuals listed
 * on the ChangeLog entries.
 *
 * Authors :
 *   Jb Evain   (jbevain@gmail.com)
 *
 * This is a free software distributed under a MIT/X11 license
 * See LICENSE.MIT file for more details
 *
 *****************************************************************************/

namespace Mono.Cecil.Implem {

	using System.Reflection;

	using Mono.Cecil;

	internal sealed class MethodReturnType : IMethodReturnType {

		private MethodReference m_method;
		private ParameterDefinition m_param;

		private ITypeReference m_returnType;

		public MethodReference Method {
			get { return m_method; }
			set { m_method = value; }
		}

		public ITypeReference ReturnType {
			get { return m_returnType; }
			set { m_returnType = value; }
		}

		public ParameterDefinition Parameter {
			get { return m_param; }
			set { m_param = value; }
		}

		public ICustomAttributeCollection CustomAttributes {
			get {
				if (m_param == null) {
					m_param = new ParameterDefinition (
						string.Empty, 0, (ParamAttributes) 0, m_returnType);
					m_param.Method = m_method;
				}

				return m_param.CustomAttributes;
			}
		}

		public IMarshalSpec MarshalSpec {
			get {
				if (m_param == null)
					return null;

				return m_param.MarshalSpec;
			}
		}

		public MethodReturnType (ITypeReference retType)
		{
			m_returnType = retType;
		}
	}
}
