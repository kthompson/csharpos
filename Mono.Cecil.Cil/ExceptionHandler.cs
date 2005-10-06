//
// ExceptionHandler.cs
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

namespace Mono.Cecil.Cil {

	using Mono.Cecil;

	public sealed class ExceptionHandler : IExceptionHandler {

		private Instruction m_tryStart;
		private Instruction m_tryEnd;
		private Instruction m_filterStart;
		private Instruction m_filterEnd;
		private Instruction m_handlerStart;
		private Instruction m_handlerEnd;

		private TypeReference m_catchType;
		private ExceptionHandlerType m_type;

		public Instruction TryStart {
			get { return m_tryStart; }
			set { m_tryStart = value as Instruction; }
		}

		public Instruction TryEnd {
			get { return m_tryEnd; }
			set { m_tryEnd = value as Instruction; }
		}

		public Instruction FilterStart {
			get { return m_filterStart; }
			set { m_filterStart = value as Instruction; }
		}

		public Instruction FilterEnd {
			get { return m_filterEnd; }
			set { m_filterEnd = value as Instruction; }
		}

		public Instruction HandlerStart {
			get { return m_handlerStart; }
			set { m_handlerStart = value as Instruction; }
		}

		public Instruction HandlerEnd {
			get { return m_handlerEnd; }
			set { m_handlerEnd = value as Instruction; }
		}

		public TypeReference CatchType {
			get { return m_catchType; }
			set { m_catchType = value; }
		}

		public ExceptionHandlerType Type {
			get { return m_type; }
			set { m_type = value; }
		}

		public ExceptionHandler (ExceptionHandlerType type)
		{
			m_type = type;
		}

		public void Accept (ICodeVisitor visitor)
		{
			visitor.VisitExceptionHandler (this);
		}
	}
}
