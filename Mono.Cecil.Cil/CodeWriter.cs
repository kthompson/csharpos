//
// CodeWriter.cs
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

	using System;
	using System.Collections;

	using Mono.Cecil;
	using Mono.Cecil.Binary;
	using Mono.Cecil.Metadata;
	using Mono.Cecil.Signatures;

	class CodeWriter : BaseCodeVisitor {

		ReflectionWriter m_reflectWriter;
		MemoryBinaryWriter m_binaryWriter;
		MemoryBinaryWriter m_codeWriter;

		IDictionary m_localSigCache;

		public CodeWriter (ReflectionWriter reflectWriter, MemoryBinaryWriter writer)
		{
			m_reflectWriter = reflectWriter;
			m_binaryWriter = writer;
			m_codeWriter = new MemoryBinaryWriter ();

			m_localSigCache = new Hashtable ();
		}

		public RVA WriteMethodBody (MethodDefinition meth)
		{
			if (meth.Body == null)
				return RVA.Zero;

			RVA ret = m_reflectWriter.MetadataWriter.GetDataCursor ();
			meth.Body.Accept (this);
			return ret;
		}

		public override void VisitMethodBody (MethodBody body)
		{
			m_codeWriter.Empty ();
		}

		void WriteToken (MetadataToken token)
		{
			if (token.RID == 0)
				m_codeWriter.Write (0);
			else
				m_codeWriter.Write (token.ToUInt ());
		}

		int GetParameterIndex (MethodBody body, ParameterDefinition p)
		{
			int idx = body.Method.Parameters.IndexOf (p);
			if (idx == -1 && p == body.Method.This)
				return 0;
			if (body.Method.HasThis)
				idx++;

			return idx;
		}

		public override void VisitInstructionCollection (InstructionCollection instructions)
		{
			MethodBody body = instructions.Container;
			long start = m_codeWriter.BaseStream.Position;

			ComputeMaxStack (instructions);

			foreach (Instruction instr in instructions) {

				instr.Offset = (int) (m_codeWriter.BaseStream.Position - start);

				if (instr.OpCode.Size == 1)
					m_codeWriter.Write (instr.OpCode.Op2);
				else {
					m_codeWriter.Write (instr.OpCode.Op1);
					m_codeWriter.Write (instr.OpCode.Op2);
				}

				if (instr.OpCode.OperandType != OperandType.InlineNone &&
					instr.Operand == null)
					throw new ReflectionException ("OpCode {0} have null operand", instr.OpCode.Name);

				switch (instr.OpCode.OperandType) {
				case OperandType.InlineNone :
					break;
				case OperandType.InlineSwitch :
					Instruction [] targets = instr.Operand as Instruction [];
					for (int i = 0; i < targets.Length + 1; i++)
						m_codeWriter.Write ((uint) 0);
					break;
				case OperandType.ShortInlineBrTarget :
					m_codeWriter.Write ((byte) 0);
					break;
				case OperandType.InlineBrTarget :
					m_codeWriter.Write (0);
					break;
				case OperandType.ShortInlineI :
					if (instr.OpCode == OpCodes.Ldc_I4_S)
						m_codeWriter.Write ((sbyte) instr.Operand);
					else
						m_codeWriter.Write ((byte) instr.Operand);
					break;
				case OperandType.ShortInlineVar :
					m_codeWriter.Write ((byte) body.Variables.IndexOf (
						(VariableDefinition) instr.Operand));
					break;
				case OperandType.ShortInlineParam :
					m_codeWriter.Write ((byte) GetParameterIndex (body, (ParameterDefinition) instr.Operand));
					break;
				case OperandType.InlineSig :
					throw new NotImplementedException ();
				case OperandType.InlineI :
					m_codeWriter.Write ((int) instr.Operand);
					break;
				case OperandType.InlineVar :
					m_codeWriter.Write ((short) body.Variables.IndexOf (
						(VariableDefinition) instr.Operand));
					break;
				case OperandType.InlineParam :
					m_codeWriter.Write ((short) GetParameterIndex (
							body, (ParameterDefinition) instr.Operand));
					break;
				case OperandType.InlineI8 :
					m_codeWriter.Write ((long) instr.Operand);
					break;
				case OperandType.ShortInlineR :
					m_codeWriter.Write ((float) instr.Operand);
					break;
				case OperandType.InlineR :
					m_codeWriter.Write ((double) instr.Operand);
					break;
				case OperandType.InlineString :
					WriteToken (new MetadataToken (TokenType.String,
							m_reflectWriter.MetadataWriter.AddUserString (instr.Operand as string)));
					break;
				case OperandType.InlineField :
					if (instr.Operand is FieldReference)
						WriteToken ((instr.Operand as FieldReference).MetadataToken);
					else
						throw new ReflectionException ("Wrong operand for InlineField: {0}",
							instr.Operand.GetType ().FullName);
					break;
				case OperandType.InlineMethod :
					if (instr.Operand is GenericInstanceMethod)
						WriteToken (m_reflectWriter.GetMethodSpecToken (instr.Operand as GenericInstanceMethod));
					else if (instr.Operand is MethodReference)
						WriteToken ((instr.Operand as MethodReference).MetadataToken);
					else
						throw new ReflectionException ("Wrong operand for InlineMethod: {0}",
							instr.Operand.GetType ().FullName);
					break;
				case OperandType.InlineType :
					if (instr.Operand is TypeReference)
						WriteToken (m_reflectWriter.GetTypeDefOrRefToken (
								instr.Operand as TypeReference));
					else
						throw new ReflectionException ("Wrong operand for InlineType: {0}",
							instr.Operand.GetType ().FullName);
					break;

				case OperandType.InlineTok :
					if (instr.Operand is TypeReference)
						WriteToken (m_reflectWriter.GetTypeDefOrRefToken (
								instr.Operand as TypeReference));
					else if (instr.Operand is GenericInstanceMethod)
						WriteToken (m_reflectWriter.GetMethodSpecToken (instr.Operand as GenericInstanceMethod));
					else if (instr.Operand is IMetadataTokenProvider)
						WriteToken ((instr.Operand as IMetadataTokenProvider).MetadataToken);
					else
						throw new ReflectionException (
							string.Format ("Wrong operand for {0} OpCode: {1}",
								instr.OpCode.OperandType.ToString (),
								instr.Operand.GetType ().FullName));
					break;
				}
			}

			// patch branches
			long pos = m_codeWriter.BaseStream.Position;

			foreach (Instruction instr in instructions) {
				switch (instr.OpCode.OperandType) {
				case OperandType.InlineSwitch :
					m_codeWriter.BaseStream.Position = instr.Offset + instr.OpCode.Size;
					Instruction [] targets = instr.Operand as Instruction [];
					m_codeWriter.Write ((uint) targets.Length);
					foreach (Instruction tgt in targets)
						m_codeWriter.Write ((tgt.Offset - (instr.Offset +
							instr.OpCode.Size + (4 * (targets.Length + 1)))));
					break;
				case OperandType.ShortInlineBrTarget :
					m_codeWriter.BaseStream.Position = instr.Offset + instr.OpCode.Size;
					m_codeWriter.Write ((byte) ((instr.Operand as Instruction).Offset -
						(instr.Offset + instr.OpCode.Size + 1)));
					break;
				case OperandType.InlineBrTarget :
					m_codeWriter.BaseStream.Position = instr.Offset + instr.OpCode.Size;
					m_codeWriter.Write ((instr.Operand as Instruction).Offset -
						(instr.Offset + instr.OpCode.Size + 4));
					break;
				}
			}

			m_codeWriter.BaseStream.Position = pos;
		}

		int GetLength (Instruction start, Instruction end, Instruction last)
		{
			return (end == null ? last.Offset + last.OpCode.Size : end.Offset) - start.Offset;
		}

		bool IsRangeFat (Instruction start, Instruction end, Instruction last)
		{
			return GetLength (start, end, last) >= 256 ||
				start.Offset >= 65536;
		}

		bool IsFat (ExceptionHandlerCollection seh, Instruction last)
		{
			for (int i = 0; i < seh.Count; i++) {
				ExceptionHandler eh = seh [i];
				if (IsRangeFat (eh.TryStart, eh.TryEnd, last))
					return true;

				switch (eh.Type) {
				case ExceptionHandlerType.Catch :
				case ExceptionHandlerType.Fault :
				case ExceptionHandlerType.Finally :
					if (IsRangeFat (eh.HandlerStart, eh.HandlerEnd, last))
						return true;
					break;
				case ExceptionHandlerType.Filter :
					if (IsRangeFat (eh.FilterStart, eh.FilterEnd, last))
						return true;
					break;
				}
			}

			return false;
		}

		void WriteExceptionHandlerCollection (ExceptionHandlerCollection seh)
		{
			m_codeWriter.QuadAlign ();
			Instruction last = seh.Container.Instructions [seh.Container.Instructions.Count - 1];

			if (!IsFat (seh, last)) {
				m_codeWriter.Write ((byte) MethodDataSection.EHTable);
				m_codeWriter.Write ((byte) (seh.Count * 12 + 4));
				m_codeWriter.Write (new byte [2]);
				foreach (ExceptionHandler eh in seh) {
					m_codeWriter.Write ((ushort) eh.Type);
					m_codeWriter.Write ((ushort) eh.TryStart.Offset);
					m_codeWriter.Write ((byte) (eh.TryEnd.Offset - eh.TryStart.Offset));
					m_codeWriter.Write ((ushort) eh.HandlerStart.Offset);
					m_codeWriter.Write ((byte) GetLength (eh.HandlerStart, eh.HandlerEnd, last));
					WriteHandlerSpecific (eh);
				}
			} else {
				m_codeWriter.Write ((byte) (MethodDataSection.FatFormat | MethodDataSection.EHTable));
				WriteFatBlockSize (seh);
				foreach (ExceptionHandler eh in seh) {
					m_codeWriter.Write ((uint) eh.Type);
					m_codeWriter.Write ((uint) eh.TryStart.Offset);
					m_codeWriter.Write ((uint) (eh.TryEnd.Offset - eh.TryStart.Offset));
					m_codeWriter.Write ((uint) eh.HandlerStart.Offset);
					m_codeWriter.Write ((uint) GetLength (eh.HandlerStart, eh.HandlerEnd, last));
					WriteHandlerSpecific (eh);
				}
			}
		}

		void WriteFatBlockSize (ExceptionHandlerCollection seh)
		{
			int size = seh.Count * 24 + 4;
			m_codeWriter.Write ((byte) (size & 0xff));
			m_codeWriter.Write ((byte) ((size >> 8) & 0xff));
			m_codeWriter.Write ((byte) ((size >> 16) & 0xff));
		}

		void WriteHandlerSpecific (ExceptionHandler eh)
		{
			switch (eh.Type) {
			case ExceptionHandlerType.Catch :
				WriteToken (eh.CatchType.MetadataToken);
				break;
			case ExceptionHandlerType.Filter :
				m_codeWriter.Write ((uint) eh.FilterStart.Offset);
				break;
			default :
				m_codeWriter.Write (0);
				break;
			}
		}

		public override void VisitVariableDefinitionCollection (VariableDefinitionCollection variables)
		{
			MethodBody body = variables.Container;
			uint sig = m_reflectWriter.SignatureWriter.AddLocalVarSig (
					GetLocalVarSig (variables));

			if (m_localSigCache.Contains (sig)) {
				body.LocalVarToken = (int) m_localSigCache [sig];
				return;
			}

			StandAloneSigTable sasTable = m_reflectWriter.MetadataTableWriter.GetStandAloneSigTable ();
			StandAloneSigRow sasRow = m_reflectWriter.MetadataRowWriter.CreateStandAloneSigRow (
				sig);

			sasTable.Rows.Add (sasRow);
			body.LocalVarToken = sasTable.Rows.Count;
			m_localSigCache [sig] = body.LocalVarToken;
		}

		public override void TerminateMethodBody (MethodBody body)
		{
			long pos = m_binaryWriter.BaseStream.Position;

			if (body.Variables.Count > 0 || body.ExceptionHandlers.Count > 0
				|| m_codeWriter.BaseStream.Length >= 64 || body.MaxStack > 8) {

				MethodHeader header = MethodHeader.FatFormat;
				if (body.InitLocals)
					header |= MethodHeader.InitLocals;
				if (body.ExceptionHandlers.Count > 0)
					header |= MethodHeader.MoreSects;

				m_binaryWriter.Write ((byte) header);
				m_binaryWriter.Write ((byte) 0x30); // (header size / 4) << 4
				m_binaryWriter.Write ((short) body.MaxStack);
				m_binaryWriter.Write ((int) m_codeWriter.BaseStream.Length);
				m_binaryWriter.Write (((int) TokenType.Signature | body.LocalVarToken));

				WriteExceptionHandlerCollection (body.ExceptionHandlers);
			} else 
				m_binaryWriter.Write ((byte) ((byte) MethodHeader.TinyFormat |
					m_codeWriter.BaseStream.Length << 2));

			m_binaryWriter.Write (m_codeWriter);
			m_binaryWriter.QuadAlign ();

			m_reflectWriter.MetadataWriter.AddData (
				(int) (m_binaryWriter.BaseStream.Position - pos));
		}

		LocalVarSig GetLocalVarSig (VariableDefinitionCollection vars)
		{
			LocalVarSig lvs = new LocalVarSig ();
			lvs.CallingConvention |= 0x7;
			lvs.Count = vars.Count;
			lvs.LocalVariables = new LocalVarSig.LocalVariable [lvs.Count];
			for (int i = 0; i < lvs.Count; i++) {
				LocalVarSig.LocalVariable lv = new LocalVarSig.LocalVariable ();
				TypeReference type = vars [i].VariableType;

				lv.CustomMods = m_reflectWriter.GetCustomMods (type);

				if (type is PinnedType) {
					lv.Constraint |= Constraint.Pinned;
					type = (type as PinnedType).ElementType;
				}

				if (type is ReferenceType) {
					lv.ByRef = true;
					type = (type as ReferenceType).ElementType;
				}

				lv.Type = m_reflectWriter.GetSigType (type);

				lvs.LocalVariables [i] = lv;
			}
			return lvs;
		}

		void ComputeMaxStack (InstructionCollection instructions)
		{
			InstructionCollection ehs = new InstructionCollection (null);
			foreach (ExceptionHandler eh in instructions.Container.ExceptionHandlers)
				switch (eh.Type) {
				case ExceptionHandlerType.Catch :
					ehs.Add (eh.HandlerStart);
					break;
				case ExceptionHandlerType.Filter :
					ehs.Add (eh.FilterStart);
					break;
				}

			int max = 0, current = 0;
			foreach (Instruction instr in instructions) {

				if (ehs.Contains (instr))
					current++;

				switch (instr.OpCode.StackBehaviourPush) {
				case StackBehaviour.Push1:
				case StackBehaviour.Pushi:
				case StackBehaviour.Pushi8:
				case StackBehaviour.Pushr4:
				case StackBehaviour.Pushr8:
				case StackBehaviour.Pushref:
				case StackBehaviour.Varpush:
					current++;
					break;
				case StackBehaviour.Push1_push1:
					current += 2;
					break;
				}

				if (max < current)
					max = current;

				switch (instr.OpCode.StackBehaviourPop) {
				case StackBehaviour.Varpop:
					break;
				case StackBehaviour.Pop1:
				case StackBehaviour.Popi:
				case StackBehaviour.Popref:
					current--;
					break;
				case StackBehaviour.Pop1_pop1:
				case StackBehaviour.Popi_pop1:
				case StackBehaviour.Popi_popi:
				case StackBehaviour.Popi_popi8:
				case StackBehaviour.Popi_popr4:
				case StackBehaviour.Popi_popr8:
				case StackBehaviour.Popref_pop1:
				case StackBehaviour.Popref_popi:
					current -= 2;
					break;
				case StackBehaviour.Popi_popi_popi:
				case StackBehaviour.Popref_popi_popi:
				case StackBehaviour.Popref_popi_popi8:
				case StackBehaviour.Popref_popi_popr4:
				case StackBehaviour.Popref_popi_popr8:
				case StackBehaviour.Popref_popi_popref:
					current -= 3;
					break;
				}
			}

			instructions.Container.MaxStack = max;
		}
	}
}
