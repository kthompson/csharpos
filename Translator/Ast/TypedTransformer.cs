using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cecil.Decompiler.Ast;
using Mono.Cecil;
using Mono.Cecil.Metadata;

namespace Compiler.Ast
{
    public class TypedTransformer : BaseCodeTransformer
    {
        public override ICodeNode VisitLiteralExpression(LiteralExpression node)
        {
            node = (LiteralExpression)base.VisitLiteralExpression(node);
            return new TypedLiteralExpression(node.Value);
        }

        public override ICodeNode VisitUnaryExpression(UnaryExpression node)
        {
            node = (UnaryExpression)base.VisitUnaryExpression(node);
            return new TypedUnaryExpression(node.Operator, node.Operand);
        }

        public override ICodeNode VisitBinaryExpression(BinaryExpression node)
        {
            node = (BinaryExpression)base.VisitBinaryExpression(node);
            return new TypedBinaryExpression(node.Operator, node.Left, node.Right);
        }

        public override ICodeNode VisitVariableReferenceExpression(VariableReferenceExpression node)
        {
            node = (VariableReferenceExpression)base.VisitVariableReferenceExpression(node);
            return new TypedVariableReferenceExpression(node.Variable);
        }

        public override ICodeNode VisitAssignExpression(AssignExpression node)
        {
            node = (AssignExpression)base.VisitAssignExpression(node);
            return new TypedAssignExpression(node.Target, node.Expression);
        }

        internal static ElementType GetElementType(TypeReference type)
        {
            //TODO: there has got to be a better way to do this... ?
            return GetElementType(Type.GetType(type.FullName, true));
        }

        internal static ElementType GetElementType(Type o)
        {
            return GetElementType(Type.GetTypeCode(o));
        }

        internal static ElementType GetElementType(TypeCode tc)
        {
            switch (tc)
            {
                case TypeCode.Boolean:
                    //Booleans get treated as I4 in cil
                    return ElementType.I4;
                case TypeCode.Single:
                    return ElementType.R4;
                case TypeCode.Double:
                    return ElementType.R8;
                case TypeCode.Char:
                    return ElementType.Char;
                case TypeCode.Byte:
                    return ElementType.U1;
                case TypeCode.SByte:
                    return ElementType.I1;
                case TypeCode.Int16:
                    return ElementType.I2;
                case TypeCode.UInt16:
                    return ElementType.U2;
                case TypeCode.Int32:
                    return ElementType.I4;
                case TypeCode.UInt32:
                    return ElementType.U4;
                case TypeCode.Int64:
                    return ElementType.I8;
                case TypeCode.UInt64:
                    return ElementType.U8;
                default:
                    throw new ArgumentException("tc");
            }
        }

        internal static ElementType GetElementType(object o)
        {

            if (o is ITypedCodeNode)
                return ((ITypedCodeNode) o).ElementType;

            return GetElementType(o.GetType());
        }
    }
}
