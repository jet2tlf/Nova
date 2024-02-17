using System.Collections.Generic;

namespace Nova.Syntax
{
    sealed class LiteralSyntax : ExpressionSyntax
    {
        public override SyntaxKind Kind => SyntaxKind.NumberExpression;
        public SyntaxToken LiteralToken { get; }

        public LiteralSyntax(SyntaxToken literalToken)
        {
            LiteralToken = literalToken;
        }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return LiteralToken;
        }
    }
}