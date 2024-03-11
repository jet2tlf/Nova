using System;

using Nova.Syntax;

namespace Nova.Bound
{
    internal sealed class BoundUnaryOperator
    {
        public SyntaxKind SyntaxKind { get; }
        public BoundUnaryKind Kind { get; }
        public Type OperandType { get; }
        public Type Type { get; }

        private BoundUnaryOperator(SyntaxKind syntaxKind, BoundUnaryKind kind, Type operandType)
            : this(syntaxKind, kind, operandType, operandType) {}

        private BoundUnaryOperator(SyntaxKind syntaxKind, BoundUnaryKind kind, Type operandType, Type resultType)
        {
            SyntaxKind = syntaxKind;
            Kind = kind;
            OperandType = operandType;
            Type = resultType;
        }

        private static BoundUnaryOperator[] _operators =
        {
            new BoundUnaryOperator(SyntaxKind.PlusToken, BoundUnaryKind.Identity, typeof(int)),
            new BoundUnaryOperator(SyntaxKind.MinusToken, BoundUnaryKind.Negation, typeof(int)),
            new BoundUnaryOperator(SyntaxKind.BangToken, BoundUnaryKind.LogicalNegation, typeof(bool))
        };

        public static BoundUnaryOperator Bind(SyntaxKind syntaxKind, Type operandType)
        {
            foreach (var op in _operators)
            {
                if (op.SyntaxKind == syntaxKind && op.OperandType == operandType)
                    return op;
            }

            return null;
        }
    }
}