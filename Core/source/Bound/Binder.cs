using System;
using System.Collections.Generic;
using System.Linq;

using Nova.Core;
using Nova.Syntax;

namespace Nova.Bound
{
    internal sealed class Binder
    {
        private readonly Dictionary<VariableSymbol, object> _variables;
        private readonly DiagnosticBag _diagnostics = new DiagnosticBag();

        public Binder(Dictionary<VariableSymbol, object> variables)
        {
            _variables = variables;
        }

        public DiagnosticBag Diagnostics => _diagnostics;

        public BoundExpression BindExpression(ExpressionSyntax syntax)
        {
            switch (syntax.Kind)
            {
                case SyntaxKind.ParenthesizedExpression:
                    return BindParenthesizedExpression((ParenthesizedSyntax)syntax);
                case SyntaxKind.LiteralExpression:
                    return BindLiteralExpression((LiteralSyntax)syntax);
                case SyntaxKind.NameExpression:
                    return BindNameExpression((NameSyntax)syntax);
                case SyntaxKind.AssignmentExpression:
                    return BindAssignmentExpression((AssignmentSyntax)syntax);
                case SyntaxKind.UnaryExpression:
                    return BindUnaryExpression((UnarySyntax)syntax);
                case SyntaxKind.BinaryExpression:
                    return BindBinaryExpression((BinarySyntax)syntax);
                default:
                    throw new Exception($"Unexpected syntax {syntax.Kind}");
            }
        }

        private BoundExpression BindParenthesizedExpression(ParenthesizedSyntax syntax)
        {
            return BindExpression(syntax.Expression);
        }

        private BoundExpression BindLiteralExpression(LiteralSyntax syntax)
        {
            var value = syntax.Value ?? 0;
            return new BoundLiteral(value);
        }

        private BoundExpression BindNameExpression(NameSyntax syntax)
        {
            var name = syntax.IdentifierToken.Text;
            var variable = _variables.Keys.FirstOrDefault(v => v.Name == name);

            if (variable == null)
            {
                _diagnostics.ReportUndefinedName(syntax.IdentifierToken.Span, name);
                return new BoundLiteral(0);
            }

            return new BoundVariable(variable);
        }

        private BoundExpression BindAssignmentExpression(AssignmentSyntax syntax)
        {
            var name = syntax.IdentifierToken.Text;
            var boundExpression = BindExpression(syntax.Expression);
            var existingVariable = _variables.Keys.FirstOrDefault(v => v.Name == name);

            if (existingVariable != null) _variables.Remove(existingVariable);

            var variable = new VariableSymbol(name, boundExpression.Type);
            _variables[variable] = null;

            return new BoundAssignment(variable, boundExpression);
        }

        private BoundExpression BindUnaryExpression(UnarySyntax syntax)
        {
            var boundOperand = BindExpression(syntax.Operand);
            var boundOperatorKind = BoundUnaryOperator.Bind(syntax.OperatorToken.Kind, boundOperand.Type);

            if (boundOperatorKind == null)
            {
                _diagnostics.ReportUndefinedUnaryOperator(syntax.OperatorToken.Span, syntax.OperatorToken.Text, boundOperand.Type);
                return boundOperand;
            }

            return new BoundUnary(boundOperatorKind, boundOperand);
        }

        private BoundExpression BindBinaryExpression(BinarySyntax syntax)
        {
            var boundLeft = BindExpression(syntax.Left);
            var boundRight = BindExpression(syntax.Right);
            var boundOperatorKind = BoundBinaryOperator.Bind(syntax.OperatorToken.Kind, boundLeft.Type, boundRight.Type);

            if (boundOperatorKind == null)
            {
                _diagnostics.ReportUndefinedBinaryOperator(syntax.OperatorToken.Span, syntax.OperatorToken.Text, boundLeft.Type, boundRight.Type);
                return boundLeft;
            }

            return new BoundBinary(boundLeft, boundOperatorKind, boundRight);
        }
    }
}
