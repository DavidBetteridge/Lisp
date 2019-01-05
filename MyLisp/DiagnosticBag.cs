using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace MyLisp
{
    public class DiagnosticBag
    {
        private List<Error> _errors = new List<Error>();
        private readonly string _sourceText;
        public ImmutableArray<Error> Errors => _errors.ToImmutableArray();
        public DiagnosticBag(string sourceText)
        {
            _sourceText = sourceText;
        }
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

        internal void ReportTooFewArguments(TextSpan span, CommandKind kind, string functionName)
        {
            var msg = $@"The function {functionName} was called with too few arguments. '{TextFromSpan(span)}'";
            _errors.Add(new Error(msg));
        }

        internal void ReportTooManyArguments(TextSpan span, CommandKind kind, string functionName)
        {
            var msg = $@"The function {functionName} was called with too many arguments. '{TextFromSpan(span)}'";
            _errors.Add(new Error(msg));
        }

        string TextFromSpan(TextSpan span) => _sourceText.Substring(span.Start, span.Length);

        internal void ReportWrongArgumentType(TextSpan span, string methodName, string argumentName, Type expectedType, Type actualType)
        {
            var msg = $@"The {argumentName} for {methodName} should be {expectedType} not {actualType}. '{TextFromSpan(span)}'";
            _errors.Add(new Error(msg));
        }
    }
}