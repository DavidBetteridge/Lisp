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
                    return ParsePlusCommand();

                case SyntaxKind.MinusToken:
                    return ParseMinusCommand();

                case SyntaxKind.SlashToken:
                    return ParseDivideCommand();

                case SyntaxKind.StarToken:
                    return ParseMultiplyCommand();

                case SyntaxKind.OnePlusToken:
                    return ParseOnePlusCommand();

                case SyntaxKind.OneMinusToken:
                    return ParseOneMinusCommand();

                default:
                    _diagnostics.ReportUnexpectedToken(Current.Span, Current.Kind);
                    break;
            }

            var endOfFileToken = MatchToken(SyntaxKind.EndOfFileToken);

            return null;
        }

        private OnePlusStatementSyntax ParseOnePlusCommand()
        {
            var openToken = MatchToken(SyntaxKind.OpenParenthesisToken);
            var command = MatchToken(SyntaxKind.OnePlusToken);
            var statements = ParseStatements();

            switch (statements.Count)
            {
                case 0:
                    _diagnostics.ReportTooFewArguments(Current.Span, Current.Kind, "1+");
                    break;
                case 1:
                    //All good
                    break;
                default:
                    _diagnostics.ReportTooManyArguments(Current.Span, Current.Kind, "1+");
                    break;
            }

            var endToken = MatchToken(SyntaxKind.CloseParenthesisToken);
            return new OnePlusStatementSyntax(openToken, command, statements.First(), endToken);
        }

        private OneMinusStatementSyntax ParseOneMinusCommand()
        {
            var openToken = MatchToken(SyntaxKind.OpenParenthesisToken);
            var command = MatchToken(SyntaxKind.OneMinusToken);
            var statements = ParseStatements();

            switch (statements.Count)
            {
                case 0:
                    _diagnostics.ReportTooFewArguments(Current.Span, Current.Kind, "1-");
                    break;
                case 1:
                    //All good
                    break;
                default:
                    _diagnostics.ReportTooManyArguments(Current.Span, Current.Kind, "1-");
                    break;
            }

            var endToken = MatchToken(SyntaxKind.CloseParenthesisToken);
            return new OneMinusStatementSyntax(openToken, command, statements.First(), endToken);
        }

        private PlusStatementSyntax ParsePlusCommand()
        {
            var openToken = MatchToken(SyntaxKind.OpenParenthesisToken);
            var command = MatchToken(SyntaxKind.PlusToken);
            var statements = ParseStatements();
            var endToken = MatchToken(SyntaxKind.CloseParenthesisToken);
            return new PlusStatementSyntax(openToken, command, statements.ToImmutable(), endToken);
        }

        private MinusStatementSyntax ParseMinusCommand()
        {
            var openToken = MatchToken(SyntaxKind.OpenParenthesisToken);
            var command = MatchToken(SyntaxKind.MinusToken);
            var statements = ParseStatements();
            var endToken = MatchToken(SyntaxKind.CloseParenthesisToken);
            return new MinusStatementSyntax(openToken, command, statements.ToImmutable(), endToken);
        }

        private DivideStatementSyntax ParseDivideCommand()
        {
            var openToken = MatchToken(SyntaxKind.OpenParenthesisToken);
            var command = MatchToken(SyntaxKind.SlashToken);
            var statements = ParseStatements();
            var endToken = MatchToken(SyntaxKind.CloseParenthesisToken);
            return new DivideStatementSyntax(openToken, command, statements.ToImmutable(), endToken);
        }

        private MultiplyStatementSyntax ParseMultiplyCommand()
        {
            var openToken = MatchToken(SyntaxKind.OpenParenthesisToken);
            var command = MatchToken(SyntaxKind.StarToken);
            var statements = ParseStatements();
            var endToken = MatchToken(SyntaxKind.CloseParenthesisToken);
            return new MultiplyStatementSyntax(openToken, command, statements.ToImmutable(), endToken);
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

                default:
                    _diagnostics.ReportUnexpectedToken(Current.Span, Current.Kind);
                    return null;
            }
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
