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
<% header = $headers["PEFileHeader"] %>
namespace Mono.Cecil.Binary {

	public sealed class PEFileHeader : IHeader, IBinaryVisitable {

<% header.fields.each { |f| %>		public <%=f.type%> <%=f.property_name%>;<% print("\n") } %>
		internal PEFileHeader ()
		{
		}

		public void SetDefaultValues ()
		{<% header.fields.each { |f| print("\n\t\t\t" +  f.property_name + " = " + f.default + ";") unless f.default.nil? } %>
		}

		public void Accept (IBinaryVisitor visitor)
		{
			visitor.Visit (this);
		}
	}
}
