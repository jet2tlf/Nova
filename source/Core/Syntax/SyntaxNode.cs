using System.Collections.Generic;

namespace Nova.Syntax
{
    abstract class SyntaxNode
    {
        public abstract SyntaxKind Kind { get; }

        public  abstract IEnumerable<SyntaxNode> GetChildren();
    }
}