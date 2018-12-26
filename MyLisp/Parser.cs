using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace MyLisp
{
    class Parser
    {
        private ImmutableArray<SyntaxToken> _tokens;
        private int _position;
        public Parser(string sourceText)
        {
            var lexer = new Lexer(sourceText);
            var tokens = new List<SyntaxToken>();
            SyntaxToken token;
            do
            {
                token = lexer.Lex();

                if (token.Kind != SyntaxKind.WhitespaceToken &&
                    token.Kind != SyntaxKind.BadToken)
                {
                    tokens.Add(token);
                }
            } while (token.Kind != SyntaxKind.EndOfFileToken);

            _tokens = tokens.ToImmutableArray();
        }

        private SyntaxToken Peek(int offset)
        {
            var index = _position + offset;
            if (index >= _tokens.Length)
                return _tokens[_tokens.Length - 1];

            return _tokens[index];
        }

        private SyntaxToken Current => Peek(0);

        private SyntaxToken NextToken()
        {
            var current = Current;
            _position++;
            return current;
        }


        private SyntaxToken MatchToken(SyntaxKind kind)
        {
            if (Current.Kind == kind)
                return NextToken();

            //            _diagnostics.ReportUnexpectedToken(Current.Span, Current.Kind, kind);
            return new SyntaxToken(kind, Current.Position, null, null);

        }

        public LineStatement ParseLine()
        {
            var openToken = MatchToken(SyntaxKind.OpenParenthesisToken);

            switch (Current.Kind)
            {
                case SyntaxKind.PlusToken:
                    break;
                default:
                    //            _diagnostics.ReportUnexpectedToken(Current.Span, Current.Kind, kind);
                    break;
            }
            var command = NextToken().Kind;

            var statements = ImmutableArray.CreateBuilder<StatementSyntax>();
            while (Current.Kind != SyntaxKind.CloseParenthesisToken && Current.Kind != SyntaxKind.EndOfFileToken)
            {
                var statement = ParseStatement();
                statements.Add(statement);
            }
            var endToken = MatchToken(SyntaxKind.CloseParenthesisToken);
            var endOfFileToken = MatchToken(SyntaxKind.EndOfFileToken);

            return new LineStatement(openToken, command, statements.ToImmutable(), endToken);
        }

        private StatementSyntax ParseStatement()
        {
            switch (Current.Kind)
            {
                case SyntaxKind.NumberToken:
                    return ParseNumberLiteral();
                default:
                    //            _diagnostics.ReportUnexpectedToken(Current.Span, Current.Kind, kind);
                    return null;
            }
        }

        private StatementSyntax ParseNumberLiteral()
        {
            var numberToken = MatchToken(SyntaxKind.NumberToken);
            return new LiteralExpressionSyntax(numberToken);
        }
    }
}
