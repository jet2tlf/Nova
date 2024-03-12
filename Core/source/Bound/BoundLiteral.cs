using System;

namespace Nova.Bound
{
    internal sealed class BoundLiteral : BoundExpression
    {
        public override BoundNodeKind Kind => BoundNodeKind.LiteralExpression;
        public override Type Type => Value.GetType();
        public object Value { get; }

        public BoundLiteral(object value)
        {
            Value = value;
        }
    }
}
