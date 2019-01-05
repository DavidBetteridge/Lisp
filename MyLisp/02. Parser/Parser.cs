using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace MyLisp
{
    public class Parser
    {
        public DiagnosticBag DiagnosticBag { get; } = new DiagnosticBag();
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


        private SyntaxToken MatchToken(SyntaxKind kind)
        {
            if (Current.Kind == kind)
                return NextToken();

            DiagnosticBag.ReportUnexpectedToken(Current.Span, Current.Kind, kind);
            return new SyntaxToken(kind, Current.Position, null, null);

        }

        public StatementSyntax ParseBracketedStatement()
        {
            switch (Peek(1).Kind)
            {
                case SyntaxKind.PlusToken:
                    return ParseCommand(SyntaxKind.PlusToken, CommandKind.PlusCommand);

                case SyntaxKind.MinusToken:
                    return ParseCommand(SyntaxKind.MinusToken, CommandKind.MinusCommand);

                case SyntaxKind.SlashToken:
                    return ParseCommand(SyntaxKind.SlashToken, CommandKind.DivideCommand);

                case SyntaxKind.StarToken:
                    return ParseCommand(SyntaxKind.StarToken, CommandKind.MultiplyCommand);

                case SyntaxKind.OnePlusToken:
                    return ParseCommand(SyntaxKind.OnePlusToken, CommandKind.OnePlusCommand);

                case SyntaxKind.OneMinusToken:
                    return ParseCommand(SyntaxKind.OneMinusToken, CommandKind.OneMinusCommand);

                case SyntaxKind.PercentToken:
                    return ParseCommand(SyntaxKind.PercentToken, CommandKind.DividendDivisorCommand);

                case SyntaxKind.IdentifierToken:
                    return ParseCommand(SyntaxKind.IdentifierToken, CommandKind.FunctionCall);

                //case SyntaxKind.DefFunKeyword:
                //    return ParseFunction();
                default:
                    DiagnosticBag.ReportUnexpectedToken(Peek(1).Span, Peek(1).Kind);
                    break;
            }

            var endOfFileToken = MatchToken(SyntaxKind.EndOfFileToken);

            return null;
        }

        private CommandStatementSyntax ParseCommand(SyntaxKind lexedToken, CommandKind commandToken)
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
                    DiagnosticBag.ReportUnexpectedToken(Current.Span, Current.Kind);
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
