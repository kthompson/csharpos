//
// EventDefinition.cs
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

	public sealed class EventDefinition : MemberDefinition, IEventDefinition {

		TypeReference m_eventType;
		EventAttributes m_attributes;

		CustomAttributeCollection m_customAttrs;

		MethodDefinition m_addMeth;
		MethodDefinition m_invMeth;
		MethodDefinition m_remMeth;

		public TypeReference EventType {
			get { return m_eventType; }
			set { m_eventType = value; }
		}

		public EventAttributes Attributes {
			get { return m_attributes; }
			set { m_attributes = value; }
		}

		public MethodDefinition AddMethod {
			get { return m_addMeth; }
			set { m_addMeth = value; }
		}

		public MethodDefinition InvokeMethod {
			get { return m_invMeth; }
			set { m_invMeth = value; }
		}

		public MethodDefinition RemoveMethod {
			get { return m_remMeth; }
			set { m_remMeth = value; }
		}

		public CustomAttributeCollection CustomAttributes {
			get {
				if (m_customAttrs == null)
					m_customAttrs = new CustomAttributeCollection (this);

				return m_customAttrs;
			}
		}

		public bool IsRuntimeSpecialName {
			get { return (m_attributes & EventAttributes.RTSpecialName) != 0; }
			set { m_attributes |= value ? EventAttributes.RTSpecialName : 0; }
		}

		public bool IsSpecialName {
			get { return (m_attributes & EventAttributes.SpecialName) != 0; }
			set { m_attributes |= value ? EventAttributes.SpecialName : 0; }
		}

		public EventDefinition (string name, TypeReference eventType,
			EventAttributes attrs) : base (name)
		{
			m_eventType = eventType;
			m_attributes = attrs;
		}

		public static MethodDefinition CreateAddMethod (IEventDefinition evt)
		{
			MethodDefinition add = new MethodDefinition (
				string.Concat ("add_", evt.Name), (MethodAttributes) 0, evt.EventType);
			evt.AddMethod = add;
			return add;
		}

		public static MethodDefinition CreateRemoveMethod (IEventDefinition evt)
		{
			MethodDefinition remove = new MethodDefinition (
				string.Concat ("remove_", evt.Name), (MethodAttributes) 0, evt.EventType);
			evt.RemoveMethod = remove;
			return remove;
		}

		public static MethodDefinition CreateInvokeMethod (IEventDefinition evt)
		{
			MethodDefinition raise = new MethodDefinition (
				string.Concat ("raise_", evt.Name), (MethodAttributes) 0, evt.EventType);
			evt.InvokeMethod = raise;
			return raise;
		}

		public EventDefinition Clone ()
		{
			EventDefinition ne = new EventDefinition (
				this.Name, this.EventType, this.Attributes);

			foreach (CustomAttribute ca in this.CustomAttributes)
				ne.CustomAttributes.Add (ca.Clone ());

			return ne;
		}

		public override string ToString ()
		{
			if (this.DeclaringType == null)
				return string.Concat (m_eventType.ToString (), ' ', this.Name);

			return string.Concat (m_eventType.ToString (), ' ',
				this.DeclaringType.ToString (), "::", this.Name);
		}

		public void Accept (IReflectionVisitor visitor)
		{
			visitor.VisitEventDefinition (this);

			this.CustomAttributes.Accept (visitor);
		}
	}
}