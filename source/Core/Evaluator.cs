using System;

using Nova.Bound;

namespace Nova.Core
{
    internal sealed class Evaluator
    {
        private readonly BoundExpression _root;

        public Evaluator(BoundExpression root)
        {
            _root = root;
        }

        public object Evaluate()
        {
            return EvaluateExpression(_root);
        }

        private object EvaluateExpression(BoundExpression node)
        {
            if (node is BoundLiteral n) return n.Value;

            if (node is BoundUnary u)
            {
                var operand = EvaluateExpression(u.Operand);

                switch (u.OperatorKind)
                {
                    case BoundUnaryKind.Identity:
                        return (int) operand;
                    case BoundUnaryKind.Negation:
                        return -(int) operand;
                    case BoundUnaryKind.LogicalNegation:
                        return !(bool) operand;
                    default:
                        throw new Exception($"Unexpected unary operator {u.OperatorKind}");
                }
            }

            if (node is BoundBinary b)
            {
                var left = EvaluateExpression(b.Left);
                var right = EvaluateExpression(b.Right);

                switch (b.OperatorKind)
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
                    default:
                        throw new Exception($"Unexpected binary operator {b.OperatorKind}");
                }
            }

            throw new Exception($"Unexpected node {node.Kind}");
        }
    }
}