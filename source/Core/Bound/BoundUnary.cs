using System;

namespace Nova.Bound
{
    internal sealed class BoundUnary : BoundExpression
    {
        public BoundUnaryKind OperatorKind { get; }
        public BoundExpression Operand { get; }
        public override Type Type => Operand.Type;
        public override BoundNodeKind Kind => BoundNodeKind.UnaryExpression;
        
        public BoundUnary(BoundUnaryKind operatorKind, BoundExpression operand)
        {
            OperatorKind = operatorKind;
            Operand = operand;
        }
    }
}