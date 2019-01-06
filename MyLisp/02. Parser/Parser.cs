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

        public Statement Parse()
        {
            var result = ParseBracketedStatement();
            var endOfFileToken = MatchToken(SyntaxKind.EndOfFileToken);
            return result;
        }

        private Statement ParseBracketedStatement()
        {
            var result = default(Statement);
            switch (Peek(1).Kind)
            {
                case SyntaxKind.PlusToken:
                    result = ParsePlusStatement();
                    break;

                case SyntaxKind.MinusToken:
                    result = ParseMinusStatement();
                    break;
                case SyntaxKind.SlashToken:
                    result = ParseDivideStatement();
                    break;
                case SyntaxKind.StarToken:
                    result = ParseMultiplyStatement();
                    break;
                case SyntaxKind.OnePlusToken:
                    result = ParseOnePlusStatement();
                    break;
                case SyntaxKind.OneMinusToken:
                    result = ParseOneMinusStatement();
                    break;
                case SyntaxKind.PercentToken:
                    result = ParseDividendDivisorStatement();
                    break;
                case SyntaxKind.IdentifierToken:
                    result = ParseIdentifierStatement();
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

        private Statement ParseIdentifierStatement()
        {
            var openToken = MatchToken(SyntaxKind.OpenParenthesisToken);
            var command = MatchToken(SyntaxKind.IdentifierToken);
            var statements = ParseStatements();
            var endToken = MatchToken(SyntaxKind.CloseParenthesisToken);
            var fullSpan = new TextSpan(openToken.Span.Start, endToken.Span.End - openToken.Span.Start);

            var functionName = (string)command.Value;
            switch (functionName.ToLower())
            {
                case "mod":
                    return ParseModStatement(openToken, command, statements, endToken, fullSpan);

                case "defvar":
                    return ParseDefVarStatement(openToken, command, statements, endToken, fullSpan);

                case "deffun":
                    return ParseDefFunStatement(openToken, command, statements, endToken, fullSpan);

                case "if":
                    return ParseIfStatement(openToken, command, statements, endToken, fullSpan);

                default:
                    return new FunctionCallStatement(functionName, statements);
            }
        }

        private Statement ParseDefFunStatement(SyntaxToken openToken, SyntaxToken command, ImmutableArray<Statement>.Builder statements, SyntaxToken endToken, TextSpan fullSpan)
        {
            var first = statements[0] as IdentifierStatement;
            var functionName = first.VariableName;

            //This is not good!
            var args = statements[1] as FunctionCallStatement;
            var firstArgument = new string[] { args.FunctionName };
            var otherArguments = args.Args.Cast<IdentifierStatement>().Select(s => s.VariableName.ToString());
            var allArguments = firstArgument.Union(otherArguments);

            var body = statements[2];

            return new FunctionStatement(functionName, allArguments, body);
        }

        private Statement ParseIfStatement(SyntaxToken openToken, SyntaxToken command, ImmutableArray<Statement>.Builder statements, SyntaxToken endToken, TextSpan fullSpan)
        {
            switch (statements.Count())
            {
                case 0:
                case 1:
                case 2:
                    DiagnosticBag.ReportTooFewArguments(fullSpan, CommandKind.IfCommand, "if");
                    return new EmptyStatement();
                case 3:
                    var predicate = statements[0];
                    var trueBranch = statements[1];
                    var falseBranch = statements[2];
                    return new IfStatement(predicate, trueBranch, falseBranch);
                default:
                    DiagnosticBag.ReportTooManyArguments(fullSpan, CommandKind.IfCommand, "if");
                    return new EmptyStatement();
            }
        }

        private Statement ParseDefVarStatement(SyntaxToken openToken, SyntaxToken command, ImmutableArray<Statement>.Builder statements, SyntaxToken endToken, TextSpan fullSpan)
        {
            var name = (string)((IdentifierStatement)statements[0]).VariableName;

            Statement initialValue = null;
            if (statements.Count() > 1)
                initialValue = statements[1];

            var documentation = "";
            if (statements.Count() > 2)
            {
                if (statements[2] is StringStatement str)
                    documentation = str.Value;
                else
                    DiagnosticBag.ReportWrongArgumentType(fullSpan, "DefVar", "Documentation", typeof(String), statements[2].GetType());
            }

            return new DefVarStatement(name, initialValue, documentation);
        }

        private Statement ParseModStatement(SyntaxToken openToken, SyntaxToken command, ImmutableArray<Statement>.Builder statements, SyntaxToken endToken, TextSpan fullSpan)
        {
            switch (statements.Count())
            {
                case 0:
                case 1:
                    DiagnosticBag.ReportTooFewArguments(fullSpan, CommandKind.ModCommand, "mod");
                    return new EmptyStatement();

                case 2:
                    {
                        var lhs = statements[0];
                        var rhs = statements[1];

                        return new ModStatement(lhs, rhs);
                    }
                default:
                    {
                        DiagnosticBag.ReportTooManyArguments(fullSpan, CommandKind.ModCommand, "mod");
                        var lhs = statements[0];
                        var rhs = statements[1];

                        return new ModStatement(lhs, rhs);
                    }
            }
        }

        private Statement ParseDividendDivisorStatement()
        {
            var openToken = MatchToken(SyntaxKind.OpenParenthesisToken);
            var command = MatchToken(SyntaxKind.PercentToken);
            var statements = ParseStatements();
            var endToken = MatchToken(SyntaxKind.CloseParenthesisToken);
            var fullSpan = new TextSpan(openToken.Span.Start, endToken.Span.End - openToken.Span.Start);

            switch (statements.Count())
            {
                case 0:
                case 1:
                    DiagnosticBag.ReportTooFewArguments(fullSpan, CommandKind.DividendDivisorCommand, "%");
                    return new EmptyStatement();

                case 2:
                    {
                        var lhs = statements[0];
                        var rhs = statements[1];

                        return new DividendDivisorStatement(lhs, rhs);
                    }
                default:
                    {
                        DiagnosticBag.ReportTooManyArguments(fullSpan, CommandKind.DividendDivisorCommand, "%");
                        var lhs = statements[0];
                        var rhs = statements[1];

                        return new DividendDivisorStatement(lhs, rhs);
                    }
            }
        }

        private Statement ParseOnePlusStatement()
        {
            var openToken = MatchToken(SyntaxKind.OpenParenthesisToken);
            var command = MatchToken(SyntaxKind.OnePlusToken);
            var statements = ParseStatements();
            var endToken = MatchToken(SyntaxKind.CloseParenthesisToken);
            var fullSpan = new TextSpan(openToken.Span.Start, endToken.Span.End - openToken.Span.Start);

            switch (statements.Count())
            {
                case 0:
                    DiagnosticBag.ReportTooFewArguments(fullSpan, CommandKind.OnePlusCommand, "1+");
                    return new EmptyStatement();

                case 1:
                    return new OnePlusStatement(statements[0]);

                default:
                    DiagnosticBag.ReportTooManyArguments(fullSpan, CommandKind.OnePlusCommand, "1+");
                    return new EmptyStatement();
            }
        }

        private Statement ParseOneMinusStatement()
        {
            var openToken = MatchToken(SyntaxKind.OpenParenthesisToken);
            var command = MatchToken(SyntaxKind.OneMinusToken);
            var statements = ParseStatements();
            var endToken = MatchToken(SyntaxKind.CloseParenthesisToken);
            var fullSpan = new TextSpan(openToken.Span.Start, endToken.Span.End - openToken.Span.Start);

            switch (statements.Count())
            {
                case 0:
                    DiagnosticBag.ReportTooFewArguments(fullSpan, CommandKind.OneMinusCommand, "1-");
                    return new EmptyStatement();

                case 1:
                    return new OneMinusStatement(statements[0]);

                default:
                    DiagnosticBag.ReportTooManyArguments(fullSpan, CommandKind.OneMinusCommand, "1-");
                    return new EmptyStatement();
            }
        }

        private PlusStatement ParsePlusStatement()
        {
            var openToken = MatchToken(SyntaxKind.OpenParenthesisToken);
            var command = MatchToken(SyntaxKind.PlusToken);
            var statements = ParseStatements();
            var endToken = MatchToken(SyntaxKind.CloseParenthesisToken);
            var fullSpan = new TextSpan(openToken.Span.Start, endToken.Span.End - openToken.Span.Start);
            return new PlusStatement(statements);
        }

        private MinusStatement ParseMinusStatement()
        {
            var openToken = MatchToken(SyntaxKind.OpenParenthesisToken);
            var command = MatchToken(SyntaxKind.MinusToken);
            var statements = ParseStatements();
            var endToken = MatchToken(SyntaxKind.CloseParenthesisToken);
            var fullSpan = new TextSpan(openToken.Span.Start, endToken.Span.End - openToken.Span.Start);
            return new MinusStatement(statements);
        }

        private DivideStatement ParseDivideStatement()
        {
            var openToken = MatchToken(SyntaxKind.OpenParenthesisToken);
            var command = MatchToken(SyntaxKind.SlashToken);
            var statements = ParseStatements();
            var endToken = MatchToken(SyntaxKind.CloseParenthesisToken);
            var fullSpan = new TextSpan(openToken.Span.Start, endToken.Span.End - openToken.Span.Start);
            return new DivideStatement(statements);
        }

        private MultiplyStatement ParseMultiplyStatement()
        {
            var openToken = MatchToken(SyntaxKind.OpenParenthesisToken);
            var command = MatchToken(SyntaxKind.StarToken);
            var statements = ParseStatements();
            var endToken = MatchToken(SyntaxKind.CloseParenthesisToken);
            var fullSpan = new TextSpan(openToken.Span.Start, endToken.Span.End - openToken.Span.Start);
            return new MultiplyStatement(statements);
        }
        private EmptyStatement ParseEmptyCommand()
        {
            var openToken = MatchToken(SyntaxKind.OpenParenthesisToken);
            var endToken = MatchToken(SyntaxKind.CloseParenthesisToken);
            return new EmptyStatement();
        }


        private ImmutableArray<Statement>.Builder ParseStatements()
        {
            var statements = ImmutableArray.CreateBuilder<Statement>();
            while (Current.Kind != SyntaxKind.CloseParenthesisToken && Current.Kind != SyntaxKind.EndOfFileToken)
            {
                var statement = ParseStatement();
                statements.Add(statement);
            }

            return statements;
        }

        private Statement ParseStatement()
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

        private StringStatement ParseStringToken()
        {
            var token = MatchToken(SyntaxKind.StringToken);
            return new StringStatement(token.Text);
        }

        private IdentifierStatement ParseIdentifier()
        {
            var token = MatchToken(SyntaxKind.IdentifierToken);
            return new IdentifierStatement(token.Text);
        }

        private NumericStatement ParseNumberLiteral()
        {
            var numberToken = MatchToken(SyntaxKind.IntegerNumberToken);
            return new NumericStatement((int)numberToken.Value);
        }

        private FloatingPointStatement ParseFloatingPointNumberLiteral()
        {
            var numberToken = MatchToken(SyntaxKind.FloatingPointNumberToken);
            return new FloatingPointStatement((double)numberToken.Value);
        }
    }
}
