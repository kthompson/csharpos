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
 * Generated by /CodeGen/cecil-gen.rb do not edit
 * <%=Time.now%>
 *
 *****************************************************************************/

namespace Mono.Cecil.Cil {

    using System.Collections;

    public sealed class OpCodes {

        private OpCodes()
        {
        }

<% $ops.each { |op| %>        public static readonly OpCode <%=op.field_name%> = new OpCode (
            "<%=op.name%>", <%=op.op1%>, <%=op.op2%>, <%=op.size%>, <%=op.flowcontrol%>,
            <%=op.opcodetype%>, <%=op.operandtype%>,
            <%=op.stackbehaviourpop%>, <%=op.stackbehaviourpush%>);
<% print("\n"); } %>        public sealed class Cache {

            public static readonly Cache Instance = new Cache ();

            private IDictionary m_cache;
            private OpCode [] m_oneByteOpCode;
            private OpCode [] m_twoBytesOpCodes;
                
            public OpCode this [string name] {
                get { return (OpCode) m_cache [name]; }
            }
            
            internal OpCode [] OneByteOpCode {
                get { return m_oneByteOpCode; }
            }
            
            internal OpCode [] TwoBytesOpCode {
                get { return m_twoBytesOpCodes; }
            }

            private Cache ()
            {
                m_cache = new Hashtable ();
<%
    oboc = Array.new
    tboc = Array.new
    $ops.each { |op|
        if op.op1 == "0xff"
            oboc.push(op)
        else
            tboc.push(op)
        end
    }
%>                m_oneByteOpCode = new OpCode [<%=oboc[oboc.length - 1].op2%> + 1];
                m_twoBytesOpCodes = new OpCode [<%=tboc[tboc.length - 1].op2%> + 1];

<% oboc.each { |op| %>                m_oneByteOpCode [<%=op.op2%>] = OpCodes.<%=op.field_name%>;
<% }
   tboc.each { |op| %>                m_twoBytesOpCodes [<%=op.op2%>] = OpCodes.<%=op.field_name%>;
<% } %>
<% $ops.each { |op| %>                m_cache ["<%=op.name%>"] = OpCodes.<%=op.field_name%>;
<% } %>            }
        }
    }
}
