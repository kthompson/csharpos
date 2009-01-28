using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil.Cil;

namespace Indy.IL2CPU
{
    public class AbstractCodeVisitor : AbstractVisitor<AbstractCodeVisitor>, ICodeVisitor
    {
        #region ICodeVisitor Members
        public Action<AbstractCodeVisitor, MethodBody> TerminateMethodBody;
        void ICodeVisitor.TerminateMethodBody(MethodBody body)
        {
            throw new NotImplementedException();
        }

        void ICodeVisitor.VisitExceptionHandler(ExceptionHandler eh)
        {
            throw new NotImplementedException();
        }

        void ICodeVisitor.VisitExceptionHandlerCollection(ExceptionHandlerCollection seh)
        {
            throw new NotImplementedException();
        }

        void ICodeVisitor.VisitInstruction(Instruction instr)
        {
            throw new NotImplementedException();
        }

        void ICodeVisitor.VisitInstructionCollection(InstructionCollection instructions)
        {
            throw new NotImplementedException();
        }

        void ICodeVisitor.VisitMethodBody(MethodBody body)
        {
            throw new NotImplementedException();
        }

        void ICodeVisitor.VisitScope(Scope scope)
        {
            throw new NotImplementedException();
        }

        void ICodeVisitor.VisitScopeCollection(ScopeCollection scopes)
        {
            throw new NotImplementedException();
        }

        void ICodeVisitor.VisitVariableDefinition(VariableDefinition var)
        {
            throw new NotImplementedException();
        }

        void ICodeVisitor.VisitVariableDefinitionCollection(VariableDefinitionCollection variables)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
