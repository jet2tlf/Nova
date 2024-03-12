using System;

namespace Nova.Bound
{
    internal sealed class BoundUnary : BoundExpression
    {
        public override BoundNodeKind Kind => BoundNodeKind.UnaryExpression;
        public override Type Type => Op.Type;
        public BoundUnaryOperator Op { get; }
        public BoundExpression Operand { get; }
        
        public BoundUnary(BoundUnaryOperator op, BoundExpression operand)
        {
            Op = op;
            Operand = operand;
        }
    }
}