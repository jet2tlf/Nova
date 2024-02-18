using System;

using Nova.Bound;
using Nova.Syntax;

namespace Nova.Core
{
    internal sealed class Evaluator
    {
        private readonly BoundExpression _root;

        public Evaluator(BoundExpression root)
        {
            _root = root;
        }

        public int Evaluate()
        {
            return EvaluateExpression(_root);
        }

        private int EvaluateExpression(BoundExpression node)
        {
            if (node is BoundLiteral n) return (int) n.Value;

            if (node is BoundUnary u)
            {
                var operand = EvaluateExpression(u.Operand);

                switch (u.OperatorKind)
                {
                    case BoundUnaryKind.Identity:
                        return operand;
                    case BoundUnaryKind.Negation:
                        return -operand;
                    default:
                        throw new Exception($"Unexpected binary operator {u.OperatorKind}");
                }
            }

            if (node is BoundBinary b)
            {
                var left = EvaluateExpression(b.Left);
                var right = EvaluateExpression(b.Right);

                switch (b.OperatorKind)
                {
                    case BoundBinaryKind.Addition:
                        return left + right;
                    case BoundBinaryKind.Subtraction:
                        return left - right;
                    case BoundBinaryKind.Multiplication:
                        return left * right;
                    case BoundBinaryKind.Division:
                        return left / right;
                    default:
                        throw new Exception($"Unexpected binary operator {b.OperatorKind}");
                }
            }

            throw new Exception($"Unexpected node {node.Kind}");
        }
    }
}