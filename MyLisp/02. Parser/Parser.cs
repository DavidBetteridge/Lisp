using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace MyLisp
{
    public class Parser
    {
        private readonly DiagnosticBag _diagnostics = new DiagnosticBag();
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

            _diagnostics.ReportUnexpectedToken(Current.Span, Current.Kind, kind);
            return new SyntaxToken(kind, Current.Position, null, null);

        }

        public StatementSyntax ParseBracketedStatement()
        {
            switch (Peek(1).Kind)
            {
                case SyntaxKind.PlusToken:
                    return ParseCommand(SyntaxKind.PlusToken, SyntaxKind.PlusCommand);

                case SyntaxKind.MinusToken:
                    return ParseCommand(SyntaxKind.MinusToken, SyntaxKind.MinusCommand);

                case SyntaxKind.SlashToken:
                    return ParseCommand(SyntaxKind.SlashToken, SyntaxKind.DivideCommand);

                case SyntaxKind.StarToken:
                    return ParseCommand(SyntaxKind.StarToken, SyntaxKind.MultiplyCommand);

                case SyntaxKind.OnePlusToken:
                    return ParseCommand(SyntaxKind.OnePlusToken, SyntaxKind.OnePlusCommand);

                case SyntaxKind.OneMinusToken:
                    return ParseCommand(SyntaxKind.OneMinusToken, SyntaxKind.OneMinusCommand);

                case SyntaxKind.PercentToken:
                    return ParseCommand(SyntaxKind.PercentToken, SyntaxKind.DividendDivisorCommand);

                case SyntaxKind.ModKeyword:
                    return ParseCommand(SyntaxKind.ModKeyword, SyntaxKind.ModCommand);

                case SyntaxKind.DefVarKeyword:
                    return ParseCommand(SyntaxKind.DefVarKeyword, SyntaxKind.DefVarCommand);

                default:
                    _diagnostics.ReportUnexpectedToken(Current.Span, Current.Kind);
                    break;
            }

            var endOfFileToken = MatchToken(SyntaxKind.EndOfFileToken);

            return null;
        }


        private CommandStatementSyntax ParseCommand(SyntaxKind lexedToken, SyntaxKind commandToken)
        {
            var openToken = MatchToken(SyntaxKind.OpenParenthesisToken);
            var command = MatchToken(lexedToken);
            var statements = ParseStatements();
            var endToken = MatchToken(SyntaxKind.CloseParenthesisToken);
            return new CommandStatementSyntax(openToken, command, statements.ToImmutable(), endToken, commandToken);
        }

        private ImmutableArray<StatementSyntax>.Builder ParseStatements()
        {
            var statements = ImmutableArray.CreateBuilder<StatementSyntax>();
            while (Current.Kind != SyntaxKind.CloseParenthesisToken && Current.Kind != SyntaxKind.EndOfFileToken)
            {
                var statement = ParseStatement();
                statements.Add(statement);
            }

            return statements;
        }

        private StatementSyntax ParseStatement()
        {
            switch (Current.Kind)
            {
                case SyntaxKind.OpenParenthesisToken:
                    return ParseBracketedStatement();

                case SyntaxKind.IntegerNumberToken:
                    return ParseNumberLiteral();

                case SyntaxKind.FloatingPointNumberToken:
                    return ParseFloatingPointNumberLiteral();

                case SyntaxKind.IdentifierToken:
                    return ParseIdentifier();

                default:
                    _diagnostics.ReportUnexpectedToken(Current.Span, Current.Kind);
                    return null;
            }
        }

        private StatementSyntax ParseIdentifier()
        {
            var token = MatchToken(SyntaxKind.IdentifierToken);
            return new IdentifierSyntax(token);
        }

        private StatementSyntax ParseNumberLiteral()
        {
            var numberToken = MatchToken(SyntaxKind.IntegerNumberToken);
            return new LiteralExpressionSyntax(numberToken);
        }

        private StatementSyntax ParseFloatingPointNumberLiteral()
        {
            var numberToken = MatchToken(SyntaxKind.FloatingPointNumberToken);
            return new LiteralExpressionSyntax(numberToken);
        }
    }
}
