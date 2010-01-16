using System;
using System.Collections.Generic;
using System.Text;
using Mono.Cecil.Cil;
using Mono.Cecil;

namespace VirtualExecutionSystem
{
    public class MethodRunner : BaseCodeVisitor
    {
        public Stack<object> Stack { get; private set; }

        private object[] _locals;
        private VariableDefinitionCollection _localTypes;
        private Instruction _jumpDestination;
        private MethodDefinition _activeMethod;
        private MethodBody _activeBody;
        private TypeReference _returnType;

        public override void VisitMethodBody(MethodBody body)
        {
            _activeBody = body;
            _activeMethod = body.Method;
            _returnType = _activeMethod.ReturnType.ReturnType;
            this.Stack = new Stack<object>();
        }

        public override void VisitVariableDefinitionCollection(VariableDefinitionCollection variables)
        {
            this._activeBody.InitLocals = true;
            this._localTypes = variables;
            this._locals = new object[variables.Count];
        }

        public override void VisitInstructionCollection(InstructionCollection instructions)
        {
            for (int i = 0; i < instructions.Count; i++)
            {
                if (_jumpDestination != null)
                {
                    for (int j = 0; j < instructions.Count; j++)
                    {
                        if (instructions[j].Offset == _jumpDestination.Offset)
                        {
                            i = j;
                            _jumpDestination = null;
                            break;
                        }
                    }
                    throw new InvalidOperationException("Did not jump when expected to jump.");
                }
                VisitInstruction(instructions[i]);
            }
        }

        public override void VisitInstruction(IInstruction instr)
        {
            switch (instr.OpCode.Code)
            {
                case Code.Add:
                    PerformAdd(this.Stack.Pop(), this.Stack.Pop());
                    break;
                case Code.Add_Ovf:
                    throw new NotImplementedException(instr.OpCode.Code.ToString());
                case Code.Add_Ovf_Un:
                    throw new NotImplementedException(instr.OpCode.Code.ToString());
                case Code.And:
                    PerformAnd(this.Stack.Pop(), this.Stack.Pop());                    
                    break;
                case Code.Arglist:
                    throw new NotImplementedException(instr.OpCode.Code.ToString());
                case Code.Beq:
                    if(this.Stack.Pop() == this.Stack.Pop())
                        _jumpDestination = (Instruction)instr.Operand;
                    break;
                case Code.Beq_S:
                case Code.Bge:
                case Code.Bge_S:
                case Code.Bge_Un:
                case Code.Bge_Un_S:
                case Code.Bgt:
                case Code.Bgt_S:
                case Code.Bgt_Un:
                case Code.Bgt_Un_S:
                case Code.Ble:
                case Code.Ble_S:
                case Code.Ble_Un:
                case Code.Ble_Un_S:
                case Code.Blt:
                case Code.Blt_S:
                case Code.Blt_Un:
                case Code.Blt_Un_S:
                case Code.Bne_Un:
                case Code.Bne_Un_S:
                case Code.Box:
                case Code.Br:
                case Code.Br_S:
                case Code.Break:
                case Code.Brfalse:
                case Code.Brfalse_S:
                case Code.Brtrue:
                case Code.Brtrue_S:
                case Code.Call:
                case Code.Calli:
                case Code.Callvirt:
                case Code.Castclass:
                case Code.Ceq:
                case Code.Cgt:
                case Code.Cgt_Un:
                case Code.Ckfinite:
                case Code.Clt:
                case Code.Clt_Un:
                case Code.Constrained:
                case Code.Conv_I:
                case Code.Conv_I1:
                case Code.Conv_I2:
                case Code.Conv_I4:
                case Code.Conv_I8:
                case Code.Conv_Ovf_I:
                case Code.Conv_Ovf_I1:
                case Code.Conv_Ovf_I1_Un:
                case Code.Conv_Ovf_I2:
                case Code.Conv_Ovf_I2_Un:
                case Code.Conv_Ovf_I4:
                case Code.Conv_Ovf_I4_Un:
                case Code.Conv_Ovf_I8:
                case Code.Conv_Ovf_I8_Un:
                case Code.Conv_Ovf_I_Un:
                case Code.Conv_Ovf_U:
                case Code.Conv_Ovf_U1:
                case Code.Conv_Ovf_U1_Un:
                case Code.Conv_Ovf_U2:
                case Code.Conv_Ovf_U2_Un:
                case Code.Conv_Ovf_U4:
                case Code.Conv_Ovf_U4_Un:
                case Code.Conv_Ovf_U8:
                case Code.Conv_Ovf_U8_Un:
                case Code.Conv_Ovf_U_Un:
                case Code.Conv_R4:
                case Code.Conv_R8:
                case Code.Conv_R_Un:
                case Code.Conv_U:
                case Code.Conv_U1:
                case Code.Conv_U2:
                case Code.Conv_U4:
                case Code.Conv_U8:
                case Code.Cpblk:
                case Code.Cpobj:
                case Code.Div:
                case Code.Div_Un:
                case Code.Dup:
                case Code.Endfilter:
                case Code.Endfinally:
                case Code.Initblk:
                case Code.Initobj:
                case Code.Isinst:
                case Code.Jmp:
                case Code.Ldarg:
                case Code.Ldarg_0:
                case Code.Ldarg_1:
                case Code.Ldarg_2:
                case Code.Ldarg_3:
                case Code.Ldarg_S:
                case Code.Ldarga:
                case Code.Ldarga_S:
                    throw new NotImplementedException(instr.OpCode.Code.ToString());
                case Code.Ldc_I4:
                    this.Stack.Push((Int32)instr.Operand);
                    break;
                case Code.Ldc_I4_0:
                    this.Stack.Push(0);
                    break;
                case Code.Ldc_I4_1:
                    this.Stack.Push(1);
                    break;
                case Code.Ldc_I4_2:
                    this.Stack.Push(2);
                    break;
                case Code.Ldc_I4_3:
                    this.Stack.Push(3);
                    break;
                case Code.Ldc_I4_4:
                    this.Stack.Push(4);
                    break;
                case Code.Ldc_I4_5:
                    this.Stack.Push(5);
                    break;
                case Code.Ldc_I4_6:
                    this.Stack.Push(6);
                    break;
                case Code.Ldc_I4_7:
                    this.Stack.Push(7);
                    break;
                case Code.Ldc_I4_8:
                    this.Stack.Push(8);
                    break;
                case Code.Ldc_I4_M1:
                    this.Stack.Push(-1);
                    break;                
                case Code.Ldc_I4_S:
                    this.Stack.Push(GetShortForm(instr));
                    break;
                case Code.Ldc_I8:
                case Code.Ldc_R4:
                case Code.Ldc_R8:
                case Code.Ldelem_Any:
                case Code.Ldelem_I:
                case Code.Ldelem_I1:
                case Code.Ldelem_I2:
                case Code.Ldelem_I4:
                case Code.Ldelem_I8:
                case Code.Ldelem_R4:
                case Code.Ldelem_R8:
                case Code.Ldelem_Ref:
                case Code.Ldelem_U1:
                case Code.Ldelem_U2:
                case Code.Ldelem_U4:
                case Code.Ldelema:
                case Code.Ldfld:
                case Code.Ldflda:
                case Code.Ldftn:
                case Code.Ldind_I:
                case Code.Ldind_I1:
                case Code.Ldind_I2:
                case Code.Ldind_I4:
                case Code.Ldind_I8:
                case Code.Ldind_R4:
                case Code.Ldind_R8:
                case Code.Ldind_Ref:
                case Code.Ldind_U1:
                case Code.Ldind_U2:
                case Code.Ldind_U4:
                case Code.Ldlen:
                    throw new NotImplementedException(instr.OpCode.Code.ToString());
                case Code.Ldloc:
                    LoadLocal((int)instr.Operand);
                    break;
                case Code.Ldloc_0:
                    LoadLocal(0);
                    break;
                case Code.Ldloc_1:
                    LoadLocal(1);
                    break;
                case Code.Ldloc_2:
                    LoadLocal(2);
                    break;
                case Code.Ldloc_3:
                    LoadLocal(3);
                    break;
                case Code.Ldloc_S:
                    LoadLocal(GetShortForm(instr));
                    break;
                case Code.Ldloca:
                case Code.Ldloca_S:
                case Code.Ldnull:
                case Code.Ldobj:
                case Code.Ldsfld:
                case Code.Ldsflda:
                case Code.Ldstr:
                case Code.Ldtoken:
                case Code.Ldvirtftn:
                case Code.Leave:
                case Code.Leave_S:
                case Code.Localloc:
                case Code.Mkrefany:
                case Code.Mul:
                case Code.Mul_Ovf:
                case Code.Mul_Ovf_Un:
                case Code.Neg:
                case Code.Newarr:
                case Code.Newobj:
                case Code.No:
                    throw new NotImplementedException(instr.OpCode.Code.ToString());
                case Code.Nop:
                    break;
                case Code.Not:
                case Code.Or:
                case Code.Pop:
                case Code.Readonly:
                case Code.Refanytype:
                case Code.Refanyval:
                case Code.Rem:
                case Code.Rem_Un:
                    throw new NotImplementedException(instr.OpCode.Code.ToString());
                case Code.Ret:
                    if (this.Stack.Count > 0)
                    {
                        var o = this.Stack.Peek();
                        if(o.GetType().FullName != _returnType.FullName)
                            throw new VerificationException("Stack value does not match return type.");
                    }
                    else if(_returnType.FullName != typeof(void).FullName)
                    {
                        throw new VerificationException("Stack should be empty if returning void.");
                    }
                    break;
                case Code.Rethrow:
                case Code.Shl:
                case Code.Shr:
                case Code.Shr_Un:
                case Code.Sizeof:
                case Code.Starg:
                case Code.Starg_S:
                case Code.Stelem_Any:
                case Code.Stelem_I:
                case Code.Stelem_I1:
                case Code.Stelem_I2:
                case Code.Stelem_I4:
                case Code.Stelem_I8:
                case Code.Stelem_R4:
                case Code.Stelem_R8:
                case Code.Stelem_Ref:
                case Code.Stfld:
                case Code.Stind_I:
                case Code.Stind_I1:
                case Code.Stind_I2:
                case Code.Stind_I4:
                case Code.Stind_I8:
                case Code.Stind_R4:
                case Code.Stind_R8:
                case Code.Stind_Ref:
                    throw new NotImplementedException(instr.OpCode.Code.ToString());
                case Code.Stloc_0:
                    SetLocal(0);                    
                    break;
                case Code.Stloc_1:
                    SetLocal(1);                    
                    break;
                case Code.Stloc_2:
                    SetLocal(2);                    
                    break;
                case Code.Stloc_3:
                    SetLocal(3);
                    break;
                case Code.Stloc:
                    SetLocal((int)instr.Operand);
                    break;
                case Code.Stloc_S:
                    SetLocal(GetShortForm(instr));
                    break;
                case Code.Stobj:
                case Code.Stsfld:
                case Code.Sub:
                case Code.Sub_Ovf:
                case Code.Sub_Ovf_Un:
                case Code.Switch:
                case Code.Tail:
                case Code.Throw:
                case Code.Unaligned:
                case Code.Unbox:
                case Code.Unbox_Any:
                case Code.Volatile:
                case Code.Xor:
                    throw new NotImplementedException(instr.OpCode.Code.ToString());
                default:
                    throw new NotImplementedException(instr.OpCode.Code.ToString());
            }
        }

        private static int GetShortForm(Instruction instr)
        {
            if (instr.Operand is VariableDefinition)
                return ((VariableDefinition)instr.Operand).Index;

            return ((IConvertible)instr.Operand).ToInt32(null);
        }

        private void LoadLocal(int index)
        {
            if(!_activeBody.InitLocals)
                throw new VerificationException();

            this.Stack.Push(_locals[index]);
        }

        private void SetLocal(int index)
        {
            _locals[index] = this.Stack.Pop();
        }

        private void PerformAdd(object p1, object p2)
        {
            if (p1 is Int32)
            {
                var a = (Int32)p1;
                if (p2 is Int32)
                    this.Stack.Push((Int32)(a + (Int32)p2));
                else if (p2 is int)
                    this.Stack.Push((int)(a + ((int)p2)));
                else if (p2 is IntPtr)
                    throw new NotImplementedException();
                else
                    throw new InvalidOperationException();
            }
            else if (p1 is Int64)
            {
                var a = (Int64)p1;
                if (p2 is Int64)
                    this.Stack.Push((Int64)(a + (Int64)p2));
                else
                    throw new InvalidOperationException();
            }
            else if (p1 is int)
            {
                var a = (int)p1;
                if(p2 is Int32)
                    this.Stack.Push((int)(a + (Int32)p2));
                else if (p2 is int)
                    this.Stack.Push((int)(a + (int)p2));
                else
                    throw new InvalidOperationException();
            }
            else if (p1 is float && p2 is float)
            {
                this.Stack.Push((float)((float)p1 + (float)p2));
            }
            else if (p1 is IntPtr)
            {
                throw new NotImplementedException();
            }
            else
            {
                throw new InvalidOperationException();
            }

        }

        private void PerformAnd(object p1, object p2)
        {
            throw new NotImplementedException();
        }

        private void PerformJump(Instruction p)
        {
            throw new NotImplementedException();
        }
        
        public override void VisitExceptionHandlerCollection(ExceptionHandlerCollection seh)
        {
            for (int i = 0; i < seh.Count; i++)
                VisitExceptionHandler(seh[i]);
        }

        public override void VisitExceptionHandler(ExceptionHandler eh)
        {
            base.VisitExceptionHandler(eh);
        }

        public override void VisitScopeCollection(ScopeCollection scopes)
        {
            base.VisitScopeCollection(scopes);
        }

        public override void VisitScope(Scope s)
        {
            base.VisitScope(s);
        }

        public override void TerminateMethodBody(MethodBody body)
        {
            base.TerminateMethodBody(body);
        }



        /*
    visitor.VisitMethodBody(this);
    this.m_variables.Accept(visitor);
    this.m_instructions.Accept(visitor);
    this.m_exceptions.Accept(visitor);
    this.m_scopes.Accept(visitor);
    visitor.TerminateMethodBody(this);
*/
    }
}
