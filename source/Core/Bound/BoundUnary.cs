using System;

namespace Nova.Bound
{
    internal sealed class BoundUnary : BoundExpression
    {
        public override BoundNodeKind Kind => BoundNodeKind.UnaryExpression;
        public override Type Type => Operand.Type;
        public BoundUnaryKind OperatorKind { get; }
        public BoundExpression Operand { get; }

        public BoundUnary(BoundUnaryKind operatorKind, BoundExpression operand)
        {
            OperatorKind = operatorKind;
            Operand = operand;
        }
    }
}