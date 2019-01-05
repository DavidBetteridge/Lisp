using System.Collections.Generic;
using System.Collections.Immutable;

namespace MyLisp
{
    public class DiagnosticBag
    {
        private List<Error> _errors = new List<Error>();
        public ImmutableArray<Error> Errors => _errors.ToImmutableArray();

        internal void AppendErrors(ImmutableArray<Error> errors)
        {
            foreach (var error in errors)
                _errors.Add(error);
        }
        internal void ReportUnterminatedString(int start, string text)
        {
            var msg = @"The string '" + text + "' is not terminated";
            _errors.Add(new Error(msg));
        }

        internal void ReportUnexpectedToken(TextSpan span, SyntaxKind actual, SyntaxKind expecting)
        {
            var msg = $@"Was expecting a {expecting} but got a {actual}.";
            _errors.Add(new Error(msg));
        }

        internal void ReportUnexpectedToken(TextSpan span, SyntaxKind kind)
        {
            var msg = $@"Was given an unexpected {kind}";
            _errors.Add(new Error(msg));
        }

    }
}