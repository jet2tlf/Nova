using System;
using System.Collections.Generic;
using Nova.Bound;

namespace Nova.Core
{
    internal sealed class Evaluator
    {
        private readonly BoundExpression _root;
        private readonly Dictionary<VariableSymbol, object> _variables;

        public Evaluator(BoundExpression root, Dictionary<VariableSymbol, object> variables)
        {
            _root = root;
            _variables = variables;
        }

        public object Evaluate()
        {
            return EvaluateExpression(_root);
        }

        private object EvaluateExpression(BoundExpression node)
        {
            if (node is BoundLiteral n) return n.Value;

            if (node is BoundVariable v) return _variables[v.Variable];

            if (node is BoundAssignment a)
            {
                var value = EvaluateExpression(a.Expression);
                _variables[a.Variable] = value;
                return value;
            }

            if (node is BoundUnary u)
            {
                var operand = EvaluateExpression(u.Operand);

                switch (u.Op.Kind)
                {
                    case BoundUnaryKind.Identity:
                        return (int) operand;
                    case BoundUnaryKind.Negation:
                        return -(int) operand;
                    case BoundUnaryKind.LogicalNegation:
                        return !(bool) operand;
                    default:
                        throw new Exception($"Unexpected unary operator {u.Op}");
                }
            }

            if (node is BoundBinary b)
            {
                var left = EvaluateExpression(b.Left);
                var right = EvaluateExpression(b.Right);

                switch (b.Op.Kind)
                {
                    case BoundBinaryKind.Addition:
                        return (int) left + (int) right;
                    case BoundBinaryKind.Subtraction:
                        return (int) left - (int) right;
                    case BoundBinaryKind.Multiplication:
                        return (int) left * (int) right;
                    case BoundBinaryKind.Division:
                        return (int) left / (int) right;
                    case BoundBinaryKind.LogicalAnd:
                        return (bool) left && (bool) right;
                    case BoundBinaryKind.LogicalOr:
                        return (bool) left || (bool) right;
                    case BoundBinaryKind.Equals:
                        return Equals(left, right);
                    case BoundBinaryKind.NotEquals:
                        return !Equals(left, right);
                    default:
                        throw new Exception($"Unexpected binary operator {b.Op}");
                }
            }

            throw new Exception($"Unexpected node {node.Kind}");
        }
    }
}