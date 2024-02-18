using System;

namespace Nova.Bound
{
    internal sealed class BoundBinary : BoundExpression
    {
        public override BoundNodeKind Kind => BoundNodeKind.UnaryExpression;
        public override Type Type => Left.Type;
        public BoundExpression Left { get; }
        public BoundBinaryKind OperatorKind { get; }
        public BoundExpression Right { get; }

        public BoundBinary(BoundExpression left, BoundBinaryKind operatorKind, BoundExpression right)
        {
            Left = left;
            OperatorKind = operatorKind;
            Right = right;
        }
    }
}