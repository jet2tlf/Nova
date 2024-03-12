using System;
using Nova.Core;

namespace Nova.Bound
{
    internal sealed class BoundAssignment : BoundExpression
    {
        public override BoundNodeKind Kind => BoundNodeKind.AssignmentExpression;
        public override Type Type => Expression.Type;
        public VariableSymbol Variable { get; }
        public BoundExpression Expression { get; }
        
        public BoundAssignment(VariableSymbol variable, BoundExpression expression)
        {
            Variable = variable;
            Expression = expression;
        }
    }
}
