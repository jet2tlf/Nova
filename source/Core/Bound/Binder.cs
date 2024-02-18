using System;
using System.Collections.Generic;

using Nova.Syntax;

namespace Nova.Bound
{
    internal sealed class Binder
    {
        private readonly List<string> _diagnostics = new List<string>();

        public IEnumerable<string> Diagnostics => _diagnostics;

        public BoundExpression BindExpression(ExpressionSyntax syntax)
        {
            switch (syntax.Kind)
            {
                case SyntaxKind.LiteralExpression:
                    return BindLiteralExpression((LiteralSyntax)syntax);
                case SyntaxKind.UnaryExpression:
                    return BindUnaryExpression((UnarySyntax)syntax);
                case SyntaxKind.BinaryExpression:
                    return BindBinaryExpression((BinarySyntax)syntax);
                default:
                    throw new Exception($"Unexpected syntax {syntax.Kind}");
            }
        }

        private BoundExpression BindLiteralExpression(LiteralSyntax syntax)
        {
            var value = syntax.Value ?? 0;
            return new BoundLiteral(value);
        }

        private BoundExpression BindUnaryExpression(UnarySyntax syntax)
        {
            var boundOperand = BindExpression(syntax.Operand);
            var boundOperatorKind = BindUnaryOperatorKind(syntax.OperatorToken.Kind, boundOperand.Type);

            if (boundOperatorKind == null)
            {
                _diagnostics.Add($"Unary operator '{syntax.OperatorToken.Text}' is not defined for type {boundOperand.Type}.");
                return boundOperand;
            }

            return new BoundUnary(boundOperatorKind.Value, boundOperand);
        }

        private BoundExpression BindBinaryExpression(BinarySyntax syntax)
        {
            var boundLeft = BindExpression(syntax.Left);
            var boundRight = BindExpression(syntax.Right);
            var boundOperatorKind = BindBinaryOperatorKind(syntax.OperatorToken.Kind, boundLeft.Type, boundRight.Type);

            if (boundOperatorKind == null)
            {
                _diagnostics.Add($"Binary operator '{syntax.OperatorToken.Text}' is not defined for types {boundLeft.Type} and {boundRight.Type}.");
                return boundLeft;
            }

            return new BoundBinary(boundLeft, boundOperatorKind.Value, boundRight);
        }

        private BoundUnaryKind? BindUnaryOperatorKind(SyntaxKind kind, Type operandType)
        {
            if (operandType == typeof(int))
            {
                switch (kind)
                {
                    case SyntaxKind.PlusToken:
                        return BoundUnaryKind.Identity;
                    case SyntaxKind.MinusToken:
                        return BoundUnaryKind.Negation;
                }
            }

            if (operandType == typeof(bool))
            {
                switch (kind)
                {
                    case SyntaxKind.BangToken:
                        return BoundUnaryKind.LogicalNegation;
                }
            }

            return  null;
        }

        private BoundBinaryKind? BindBinaryOperatorKind(SyntaxKind kind, Type leftType, Type rightType)
        {
            if (leftType == typeof(int) && rightType == typeof(int))
            {
                switch (kind)
                {
                    case SyntaxKind.PlusToken:
                        return BoundBinaryKind.Addition;
                    case SyntaxKind.MinusToken:
                        return BoundBinaryKind.Subtraction;
                    case SyntaxKind.StarToken:
                        return BoundBinaryKind.Multiplication;
                    case SyntaxKind.SlashToken:
                        return BoundBinaryKind.Division;
                }
            }

            if (leftType == typeof(bool) && rightType == typeof(bool))
            {
                switch (kind)
                {
                    case SyntaxKind.AmpersandAmpersandToken:
                        return BoundBinaryKind.LogicalAnd;
                    case SyntaxKind.PipePipeToken:
                        return BoundBinaryKind.LogicalOr;
                }
            }

            return null;
        }
    }
}
