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
 * Generated by /CodeGen/cecil-gen.rb do not edit
 * <%=Time.now%>
 *
 *****************************************************************************/

namespace Mono.Cecil.Implem {

	using System;
	using System.Collections;

	using Mono.Cecil;
	using Mono.Cecil.Cil;

	internal class <%=$cur_coll.name%> : <%=$cur_coll.intf%>, ILazyLoadableCollection {

		private IList m_items;
		private <%=$cur_coll.container_impl%> m_container;
		private ReflectionController m_controller;

		private bool m_loaded;

		public event <%=$cur_coll.item_name%>EventHandler On<%=$cur_coll.item_name%>Added;
		public event <%=$cur_coll.item_name%>EventHandler On<%=$cur_coll.item_name%>Removed;

		public <%=$cur_coll.type%> this [int index] {
			get {
				return m_items [index] as <%=$cur_coll.type%>;
			}
			set { m_items [index] = value; }
		}

		public <%=$cur_coll.container%> Container {
			get { return m_container; }
		}

		public int Count {
			get { return m_items.Count; }
		}

		public bool IsSynchronized {
			get { return false; }
		}

		public object SyncRoot {
			get { return this; }
		}

		public bool Loaded {
			get { return m_loaded; }
			set { m_loaded = value; }
		}

		public <%=$cur_coll.name%> (<%=$cur_coll.container_impl%> container)
		{
			m_container = container;
			m_items = new ArrayList ();
		}

		public <%=$cur_coll.name%> (<%=$cur_coll.container_impl%> container, ReflectionController controller) : this (container)
		{
			m_controller = controller;
		}

		public void Add (<%=$cur_coll.type%> value)
		{
			if (On<%=$cur_coll.item_name%>Added != null && !this.Contains (value))
				On<%=$cur_coll.item_name%>Added (this, new <%=$cur_coll.item_name%>EventArgs (value));
			m_items.Add (value);
		}

		public void Clear ()
		{
			if (On<%=$cur_coll.item_name%>Removed != null)
				foreach (<%=$cur_coll.type%> item in this)
					On<%=$cur_coll.item_name%>Removed (this, new <%=$cur_coll.item_name%>EventArgs (item));
			m_items.Clear ();
		}

		public bool Contains (<%=$cur_coll.type%> value)
		{
			return m_items.Contains (value);
		}

		public int IndexOf (<%=$cur_coll.type%> value)
		{
			return m_items.IndexOf (value);
		}

		public void Insert (int index, <%=$cur_coll.type%> value)
		{
			if (On<%=$cur_coll.item_name%>Added != null && !this.Contains (value))
				On<%=$cur_coll.item_name%>Added (this, new <%=$cur_coll.item_name%>EventArgs (value));
			m_items.Insert (index, value);
		}

		public void Remove (<%=$cur_coll.type%> value)
		{
			if (On<%=$cur_coll.item_name%>Removed != null && this.Contains (value))
				On<%=$cur_coll.item_name%>Removed (this, new <%=$cur_coll.item_name%>EventArgs (value));
			m_items.Remove (value);
		}

		public void RemoveAt (int index)
		{
			if (On<%=$cur_coll.item_name%>Removed != null)
				On<%=$cur_coll.item_name%>Removed (this, new <%=$cur_coll.item_name%>EventArgs (this [index]));
			m_items.Remove (index);
		}

		public void CopyTo (Array ary, int index)
		{
			m_items.CopyTo (ary, index);
		}

		public IEnumerator GetEnumerator ()
		{
			return m_items.GetEnumerator ();
		}

		public void Load ()
		{
			if (m_controller != null && !m_loaded) {
				m_controller.<%=$cur_coll.pathtoloader%>.<%=$cur_coll.visitThis%> (this);
				m_loaded = true;
			}
		}
<%
	case $cur_coll.item_name
		when "MethodDefinition"
%>
		public IMethodDefinition [] GetMethod (string name)
		{
			ArrayList ret = new ArrayList ();
			foreach (IMethodDefinition meth in this)
				if (meth.Name == name)
					ret.Add (meth);

			return ret.ToArray (typeof (IMethodDefinition)) as IMethodDefinition [];
		}

		public IMethodDefinition GetMethod (string name, Type [] parameters)
		{
			foreach (IMethodDefinition meth in this)
				if (meth.Name == name && meth.Parameters.Count == parameters.Length)
					for (int i = 0; i < parameters.Length; i++)
						if (meth.Parameters [i].ParameterType.FullName == m_controller.Helper.GetTypeSignature (parameters [i]))
							return meth;

			return null;
		}

		public IMethodDefinition GetMethod (string name, ITypeReference [] parameters)
		{
			foreach (IMethodDefinition meth in this)
				if (meth.Name == name && meth.Parameters.Count == parameters.Length)
					for (int i = 0; i < parameters.Length; i++)
						if (meth.Parameters [i].ParameterType.FullName == parameters [i].FullName)
							return meth;

			return null;
		}
<%
		when "FieldDefinition"
%>
		public IFieldDefinition GetField (string name)
		{
			foreach (IFieldDefinition field in this)
				if (field.Name == name)
					return field;

			return null;
		}
<%
		when "Constructor"
%>
		public IMethodDefinition GetConstructor (bool isStatic, Type [] parameters)
		{
			foreach (IMethodDefinition ctor in this)
				if (ctor.IsStatic == isStatic && ctor.Parameters.Count == parameters.Length)
					for (int i = 0; i < parameters.Length; i++)
						if (ctor.Parameters [i].ParameterType.FullName == m_controller.Helper.GetTypeSignature (parameters [i]))
							return ctor;

			return null;
		}

		public IMethodDefinition GetConstructor (bool isStatic, ITypeReference [] parameters)
		{
			foreach (IMethodDefinition ctor in this)
				if (ctor.IsStatic == isStatic && ctor.Parameters.Count == parameters.Length)
					for (int i = 0; i < parameters.Length; i++)
						if (ctor.Parameters [i].ParameterType.FullName == parameters [i].FullName)
							return ctor;

			return null;
		}
<%
		when "EventDefinition"
%>
		public IEventDefinition GetEvent (string name)
		{
			foreach (IEventDefinition evt in this)
				if (evt.Name == name)
					return evt;

			return null;
		}
<%
		when "PropertyDefinition"
%>
		public IPropertyDefinition [] GetProperties (string name)
		{
			ArrayList ret = new ArrayList ();
			foreach (IPropertyDefinition prop in this)
				if (prop.Name == name)
					ret.Add (prop);

			return ret.ToArray (typeof (IPropertyDefinition)) as IPropertyDefinition [];
		}
<% end %>
		public void Accept (<%=$cur_coll.visitor%> visitor)
		{
			visitor.<%=$cur_coll.visitThis%> (this);
		}
	}
}
