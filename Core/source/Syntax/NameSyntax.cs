using System.Collections.Generic;

namespace Nova.Syntax
{
    public sealed class NameSyntax : ExpressionSyntax
    {   
        public override SyntaxKind Kind => SyntaxKind.NameExpression;
        public SyntaxToken IdentifierToken { get; }

        public NameSyntax(SyntaxToken identifierToken)
        {
            IdentifierToken = identifierToken;
        }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return IdentifierToken;
        }
    }
}