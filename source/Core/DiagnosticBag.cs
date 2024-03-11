using System;
using System.Collections;
using System.Collections.Generic;

using Nova.Syntax;

namespace Nova.Core
{
    internal sealed class DiagnosticBag : IEnumerable<Diagnostic>
    {
        private readonly List<Diagnostic> _diagnostics = new List<Diagnostic>();

        public IEnumerator<Diagnostic> GetEnumerator() => _diagnostics.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private void Report(TextSpan span, string message)
        {
            var diagnostic = new Diagnostic(span, message);
            _diagnostics.Add(diagnostic);
        }

        public void ReportInvalidNumber(TextSpan textSpan, string text, Type type)
        {
            var message = $"The number {text} isn't valid {type}.";
            Report(textSpan, message);
        }

        internal void ReportBadCharacter(int position, char current)
        {
            var span = new TextSpan(position, 1);
            var message = $"Bad character input: '{current}'.";
            Report(span, message);
        }

        internal void AddRange(DiagnosticBag diagnostics)
        {
            _diagnostics.AddRange(diagnostics._diagnostics);
        }

        internal void ReportUnexpectedToken(TextSpan span, SyntaxKind kind1, SyntaxKind kind2)
        {
            var message = $"Unexpected token <{kind1}>, expected <{kind2}>.";
            Report(span, message);
        }

        internal void ReportUndefinedUnaryOperator(TextSpan span, string text, Type type)
        {
            var message = $"Unary operator '{text}' is not defined for type {type}.";
            Report(span, message);
        }

        internal void ReportUndefinedBinaryOperator(TextSpan span, string text, Type type1, Type type2)
        {
            var message = $"Binary operator '{text}' is not defined for types {type1} and {type2}.";
            Report(span, message);
        }
    }
}