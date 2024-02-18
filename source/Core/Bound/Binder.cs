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
                    return BindLiteral((LiteralSyntax)syntax);
                case SyntaxKind.UnaryExpression:
                    return BindUnary((UnarySyntax)syntax);
                case SyntaxKind.BinaryExpression:
                    return BindBinary((BinarySyntax)syntax);
                default:
                    throw new Exception($"Unexpected syntax {syntax.Kind}");
            }
        }

        private BoundExpression BindLiteral(LiteralSyntax syntax)
        {
            var value = syntax.LiteralToken.Value as int? ?? 0;
            return new BoundLiteral(value);
        }

        private BoundExpression BindUnary(UnarySyntax syntax)
        {
            var boundOperand = BindExpression(syntax.Operand);
            var boundKind = BindUnaryKind(syntax.OperatorToken.Kind, boundOperand.Type);

            if (boundKind == null)
            {
                _diagnostics.Add($"Unary operator '{syntax.OperatorToken.Text}' is not defined for type {boundOperand.Type}");
                return boundOperand;
            }

            return new BoundUnary(boundKind.Value, boundOperand);
        }

        private BoundExpression BindBinary(BinarySyntax syntax)
        {
            var boundLeft = BindExpression(syntax.Left);
            var boundRight = BindExpression(syntax.Right);
            var boundKind = BindBinaryKind(syntax.OperatorToken.Kind, boundLeft.Type, boundRight.Type);

            if (boundKind == null)
            {
                _diagnostics.Add($"Binary operator '{syntax.OperatorToken.Text}' is not defined for type {boundLeft.Type} and {boundRight.Type}");
                return boundLeft;
            }

            return new BoundBinary(boundLeft, boundKind.Value, boundRight);
        }

        private BoundUnaryKind? BindUnaryKind(SyntaxKind kind, Type operandType)
        {
            if (operandType != typeof(int)) return null;

            switch (kind)
            {
                case SyntaxKind.PlusToken:
                    return BoundUnaryKind.Identity;
                case SyntaxKind.MinusToken:
                    return BoundUnaryKind.Negation;
                default:
                    throw new Exception($"Unexpected unary operator {kind}");
            }
        }

        private BoundBinaryKind? BindBinaryKind(SyntaxKind kind, Type leftType, Type rightType)
        {
            if (leftType != typeof(int) || rightType != typeof(int)) return null;

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
                default:
                    throw new Exception($"Unexpected binary operator {kind}");
            }
        }
    }
}