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

namespace Mono.Cecil {

	using System;
	using System.Collections;

	public class <%=$cur_coll.item_name%>EventArgs : EventArgs {

		private <%=$cur_coll.type%> m_item;

		public <%=$cur_coll.type%> <%=$cur_coll.item_name%> {
			get { return m_item; }
		}

		public <%=$cur_coll.item_name%>EventArgs (<%=$cur_coll.type%> item)
		{
			m_item = item;
		}
	}

	public delegate void <%=$cur_coll.item_name%>EventHandler (
		object sender, <%=$cur_coll.item_name%>EventArgs ea);

	public interface <%=$cur_coll.intf%> : ICollection<% if (!$cur_coll.visitable.nil?) then %>, <%=$cur_coll.visitable%><% end %> {

		<%=$cur_coll.type%> this [int index] { get; }
		<%=$cur_coll.type%> this [string fullName] { get; }

		<%=$cur_coll.container%> Container { get; }

		event <%=$cur_coll.item_name%>EventHandler On<%=$cur_coll.item_name%>Added;
		event <%=$cur_coll.item_name%>EventHandler On<%=$cur_coll.item_name%>Removed;

		void Add (<%=$cur_coll.type%> value);
		void Clear ();
		bool Contains (<%=$cur_coll.type%> value);
		bool Contains (string fullName);
		int IndexOf (<%=$cur_coll.type%> value);
		void Remove (<%=$cur_coll.type%> value);
		void RemoveAt (int index);
	}
}
