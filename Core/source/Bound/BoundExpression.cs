using System;

namespace Nova.Bound
{
    internal abstract class BoundExpression : BoundNode
    {
        public abstract Type Type { get; }
    }
}