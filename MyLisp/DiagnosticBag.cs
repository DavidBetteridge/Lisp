using System;
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
            throw new Exception("ReportUnexpectedToken " + kind);
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