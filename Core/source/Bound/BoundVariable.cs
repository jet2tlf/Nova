using System;
using Nova.Core;

namespace Nova.Bound
{
    internal sealed class BoundVariable : BoundExpression
    {
        public override BoundNodeKind Kind => BoundNodeKind.VariableExpression;
        public override Type Type => Variable.Type;
        public VariableSymbol Variable { get; }
        
        public BoundVariable(VariableSymbol variable)
        {
            Variable = variable;
        }
    }
}
