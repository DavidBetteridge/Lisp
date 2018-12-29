using System;

namespace MyLisp
{
    internal class DiagnosticBag
    {
        internal void ReportBadCharacter(int position, char current)
        {
            throw new NotImplementedException();
        }

        internal void ReportInvalidNumber(TextSpan textSpan, string text, Type type)
        {
            throw new NotImplementedException();
        }

        internal void ReportUnexpectedToken(object span, SyntaxKind kind1, SyntaxKind kind2)
        {
            throw new NotImplementedException();
        }

        internal void ReportUnexpectedToken(TextSpan span, SyntaxKind kind)
        {
            throw new NotImplementedException();
        }

        internal void ReportTooManyArguments(TextSpan span, SyntaxKind kind, string v)
        {
            throw new NotImplementedException();
        }

        internal void ReportTooFewArguments(TextSpan span, SyntaxKind kind, string v)
        {
            throw new NotImplementedException();
        }
    }
}