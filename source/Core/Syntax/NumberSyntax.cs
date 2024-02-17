using System.Collections.Generic;

namespace Nova.Syntax
{
    sealed class NumberSyntax : ExpressionSyntax
    {
        public SyntaxToken NumberToken { get; }

        public NumberSyntax(SyntaxToken numberToken)
        {
            NumberToken = numberToken;
        }

        public override SyntaxKind Kind => SyntaxKind.NumberExpression;

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return NumberToken;
        }
    }
}