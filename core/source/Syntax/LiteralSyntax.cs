using System.Collections.Generic;

namespace Nova.Syntax
{
    public sealed class LiteralSyntax : ExpressionSyntax
    {
        public override SyntaxKind Kind => SyntaxKind.LiteralExpression;
        public SyntaxToken LiteralToken { get; }
        public object Value { get; }

        public LiteralSyntax(SyntaxToken literalToken) : this(literalToken, literalToken.Value) { }

        public LiteralSyntax(SyntaxToken literalToken, object value)
        {
            LiteralToken = literalToken;
            Value = value;
        }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return LiteralToken;
        }
    }
}