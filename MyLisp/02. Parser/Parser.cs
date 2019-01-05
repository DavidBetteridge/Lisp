using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace MyLisp
{
    public class Parser
    {
        public DiagnosticBag DiagnosticBag { get; }
        private ImmutableArray<SyntaxToken> _tokens;
        private int _position;
        public Parser(string sourceText)
        {
            DiagnosticBag = new DiagnosticBag(sourceText);

            var lexer = new Lexer(sourceText);
            var tokens = new List<SyntaxToken>();
            SyntaxToken token;
            do
            {
                token = lexer.Lex();

                if (token.Kind != SyntaxKind.BadToken)
                {
                    tokens.Add(token);
                }
            } while (token.Kind != SyntaxKind.EndOfFileToken);

            _tokens = tokens.ToImmutableArray();

            if (lexer.DiagnosticBag.Errors.Any())
                DiagnosticBag.AppendErrors(lexer.DiagnosticBag.Errors);

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


        private SyntaxToken MatchToken(SyntaxKind expecting)
        {
            if (Current.Kind == expecting)
                return NextToken();

            DiagnosticBag.ReportUnexpectedToken(Current.Span, Current.Kind, expecting);
            return new SyntaxToken(expecting, Current.Position, null, null);

        }

        public StatementSyntax Parse()
        {
            var result = ParseBracketedStatement();
            var endOfFileToken = MatchToken(SyntaxKind.EndOfFileToken);
            return result;
        }

        private StatementSyntax ParseBracketedStatement()
        {
            var result = default(StatementSyntax);
            switch (Peek(1).Kind)
            {
                case SyntaxKind.PlusToken:
                    result = ParseCommand(SyntaxKind.PlusToken, CommandKind.PlusCommand);
                    break;

                case SyntaxKind.MinusToken:
                    result = ParseCommand(SyntaxKind.MinusToken, CommandKind.MinusCommand);
                    break;
                case SyntaxKind.SlashToken:
                    result = ParseCommand(SyntaxKind.SlashToken, CommandKind.DivideCommand);
                    break;
                case SyntaxKind.StarToken:
                    result = ParseCommand(SyntaxKind.StarToken, CommandKind.MultiplyCommand);
                    break;
                case SyntaxKind.OnePlusToken:
                    result = ParseCommand(SyntaxKind.OnePlusToken, CommandKind.OnePlusCommand);
                    break;
                case SyntaxKind.OneMinusToken:
                    result = ParseCommand(SyntaxKind.OneMinusToken, CommandKind.OneMinusCommand);
                    break;
                case SyntaxKind.PercentToken:
                    result = ParseCommand(SyntaxKind.PercentToken, CommandKind.DividendDivisorCommand);
                    break;
                case SyntaxKind.IdentifierToken:
                    result = ParseCommand(SyntaxKind.IdentifierToken, CommandKind.FunctionCall);
                    break;
                case SyntaxKind.CloseParenthesisToken:
                    result = ParseEmptyCommand();
                    break;
                default:
                    DiagnosticBag.ReportUnexpectedToken(Peek(1).Span, Peek(1).Kind);
                    break;
            }

            return result;
        }

        private EmptyStatementSyntax ParseEmptyCommand()
        {
            var openToken = MatchToken(SyntaxKind.OpenParenthesisToken);
            var endToken = MatchToken(SyntaxKind.CloseParenthesisToken);
            return new EmptyStatementSyntax(openToken, endToken);
        }

        private CommandStatementSyntax ParseCommand(SyntaxKind lexedToken, CommandKind commandToken)
        {
            var openToken = MatchToken(SyntaxKind.OpenParenthesisToken);
            var command = MatchToken(lexedToken);
            var statements = ParseStatements();
            var endToken = MatchToken(SyntaxKind.CloseParenthesisToken);
            var fullSpan = new TextSpan(openToken.Span.Start, endToken.Span.End - openToken.Span.Start);
            return new CommandStatementSyntax(openToken, command, statements.ToImmutable(), endToken, commandToken, fullSpan);
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

                case SyntaxKind.StringToken:
                    return ParseStringToken();

                default:
                    DiagnosticBag.ReportUnexpectedToken(Current.Span, Current.Kind);
                    NextToken();
                    return null;
            }
        }

        private StatementSyntax ParseStringToken()
        {
            var token = MatchToken(SyntaxKind.StringToken);
            return new StringSyntax(token);
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
