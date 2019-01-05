using System;
using System.Linq;

namespace MyLisp
{
    public class Binder
    {
        public DiagnosticBag DiagnosticBag { get; }

        public Binder(string sourceText)
        {
            DiagnosticBag = new DiagnosticBag(sourceText);
        }

        public BoundStatement Bind(StatementSyntax statement)
        {
            switch (statement.Kind)
            {
                case CommandKind.Empty:
                    return BindEmpty((EmptyStatementSyntax)statement);

                case CommandKind.FunctionCall:
                    return BindFunctionCall((CommandStatementSyntax)statement);

                case CommandKind.OnePlusCommand:
                    return BindOnePlusStatement((CommandStatementSyntax)statement);

                case CommandKind.OneMinusCommand:
                    return BindOneMinusStatement((CommandStatementSyntax)statement);

                case CommandKind.PlusCommand:
                    return BindPlusStatement((CommandStatementSyntax)statement);

                case CommandKind.MinusCommand:
                    return BindMinusStatement((CommandStatementSyntax)statement);

                case CommandKind.MultiplyCommand:
                    return BindMultiplyStatement((CommandStatementSyntax)statement);

                case CommandKind.DivideCommand:
                    return BindDivideStatement((CommandStatementSyntax)statement);

                case CommandKind.DividendDivisorCommand:
                    return BindDividendDivisorStatement((CommandStatementSyntax)statement);

                case CommandKind.LiteralExpression:
                    return BindLiteralExpression((LiteralExpressionSyntax)statement);

                case CommandKind.IdentifierExpression:
                    return BindIdentifier((IdentifierSyntax)statement);

                default:
                    throw new Exception($"Unexpected syntax {statement.Kind}");
            }
        }

        private BoundEmptyStatement BindEmpty(EmptyStatementSyntax statement)
        {
            return new BoundEmptyStatement();
        }

        private BoundIdentifierStatement BindIdentifier(IdentifierSyntax statement)
        {
            var variableName = (string)statement.Value;
            return new BoundIdentifierStatement(variableName);
        }

        private BoundStatement BindFunctionCall(CommandStatementSyntax statement)
        {
            var functionName = (string)statement.Command.Value;

            switch (functionName.ToLower())
            {
                case "mod":
                    return BindModStatement(statement);

                case "defvar":
                    return BindDefVarStatement(statement);

                case "deffun":
                    return BindDefFunStatement(statement);

                case "if":
                    return BindIfStatement(statement);

                default:
                    var args = statement.Statements.Select(Bind);
                    return new BoundFunctionCallStatement(functionName, args);
            }
        }

        private BoundStatement BindIfStatement(CommandStatementSyntax statement)
        {
            //(if predicate true false)
            var boundStatements = statement.Statements.Select(stat => Bind(stat)).ToArray();
            switch (boundStatements.Length)
            {
                case 0:
                case 1:
                case 2:
                    DiagnosticBag.ReportTooFewArguments(statement.Span, statement.Kind, "if");
                    return new BoundEmptyStatement();
                case 3:
                    var predicate = boundStatements[0];
                    var trueBranch = boundStatements[1];
                    var falseBranch = boundStatements[2];
                    return new BoundIfStatement(predicate, trueBranch, falseBranch);
                default:
                    DiagnosticBag.ReportTooManyArguments(statement.Span, statement.Kind, "if");
                    return new BoundEmptyStatement();
            }
        }

        private BoundFunctionStatement BindDefFunStatement(CommandStatementSyntax statement)
        {
            var first = statement.Statements[0] as IdentifierSyntax;
            var functionName = first.Value.ToString();

            var args = statement.Statements[1] as CommandStatementSyntax;
            var firstArgument = new string[] { args.Command.Value.ToString() };
            var otherArguments = args.Statements.Cast<IdentifierSyntax>().Select(s => s.Value.ToString());
            var allArguments = firstArgument.Union(otherArguments);

            var body = Bind(statement.Statements[2]);

            return new BoundFunctionStatement(functionName, allArguments, body);
        }



        private BoundDefVarStatement BindDefVarStatement(CommandStatementSyntax statement)
        {
            var name = (string)((IdentifierSyntax)statement.Statements[0]).Value;

            BoundStatement initialValue = null;
            if (statement.Statements.Count() > 1)
                initialValue = Bind(statement.Statements[1]);

            var documentation = "";
            if (statement.Statements.Count() > 2)
            {
                if (statement.Statements[2] is StringSyntax str)
                    documentation = str.Text;
                else
                    DiagnosticBag.ReportWrongArgumentType(statement.Span, "DefVar", "Documentation", typeof(String), statement.Statements[2].GetType());
            }

            return new BoundDefVarStatement(name, initialValue, documentation);
        }

        private BoundStatement BindOneMinusStatement(CommandStatementSyntax statement)
        {
            var statements = statement.Statements;

            switch (statements.Count())
            {
                case 0:
                    DiagnosticBag.ReportTooFewArguments(statement.Span, statement.Kind, "1-");
                    return new BoundEmptyStatement();

                case 1:
                    var boundStatement = Bind(statements.First());
                    return new BoundOneMinusStatement(boundStatement);

                default:
                    DiagnosticBag.ReportTooManyArguments(statement.Span, statement.Kind, "1-");
                    return new BoundEmptyStatement();
            }
        }

        private BoundStatement BindOnePlusStatement(CommandStatementSyntax statement)
        {
            var statements = statement.Statements;

            switch (statements.Count())
            {
                case 0:
                    DiagnosticBag.ReportTooFewArguments(statement.Span, statement.Kind, "1+");
                    return new BoundEmptyStatement();

                case 1:
                    var boundStatement = Bind(statements.First());
                    return new BoundOnePlusStatement(boundStatement);

                default:
                    DiagnosticBag.ReportTooManyArguments(statement.Span, statement.Kind, "1+");
                    return new BoundEmptyStatement();
            }
        }

        private BoundLiteralExpression BindLiteralExpression(LiteralExpressionSyntax statement)
        {
            return new BoundLiteralExpression(statement.Value);
        }

        private BoundPlusStatement BindPlusStatement(CommandStatementSyntax statement)
        {
            var boundStatements = statement.Statements.Select(stat => Bind(stat)).ToArray();
            return new BoundPlusStatement(boundStatements);
        }

        private BoundMinusStatement BindMinusStatement(CommandStatementSyntax statement)
        {
            var boundStatements = statement.Statements.Select(stat => Bind(stat)).ToArray();
            return new BoundMinusStatement(boundStatements);
        }

        private BoundMultiplyStatement BindMultiplyStatement(CommandStatementSyntax statement)
        {
            var boundStatements = statement.Statements.Select(stat => Bind(stat)).ToArray();
            return new BoundMultiplyStatement(boundStatements);
        }

        private BoundStatement BindDividendDivisorStatement(CommandStatementSyntax statement)
        {
            var boundStatements = statement.Statements.Select(stat => Bind(stat)).ToArray();
            switch (statement.Statements.Count())
            {
                case 0:
                case 1:
                    DiagnosticBag.ReportTooFewArguments(statement.Span, statement.Kind, "%");
                    return new BoundEmptyStatement();

                case 2:
                    {
                        var lhs = boundStatements[0];
                        var rhs = boundStatements[1];

                        return new BoundDividendDivisorStatement(lhs, rhs);
                    }
                default:
                    {
                        DiagnosticBag.ReportTooManyArguments(statement.Span, statement.Kind, "%");
                        var lhs = boundStatements[0];
                        var rhs = boundStatements[1];

                        return new BoundDividendDivisorStatement(lhs, rhs);
                    }
            }
        }

        private BoundStatement BindModStatement(CommandStatementSyntax statement)
        {
            var boundStatements = statement.Statements.Select(stat => Bind(stat)).ToArray();
            switch (statement.Statements.Count())
            {
                case 0:
                case 1:
                    DiagnosticBag.ReportTooFewArguments(statement.Span, statement.Kind, "mod");
                    return new BoundEmptyStatement();

                case 2:
                    {
                        var lhs = boundStatements[0];
                        var rhs = boundStatements[1];

                        return new BoundModStatement(lhs, rhs);
                    }
                default:
                    {
                        DiagnosticBag.ReportTooManyArguments(statement.Span, statement.Kind, "mod");
                        var lhs = boundStatements[0];
                        var rhs = boundStatements[1];

                        return new BoundModStatement(lhs, rhs);
                    }
            }
        }

        private BoundDivideStatement BindDivideStatement(CommandStatementSyntax statement)
        {
            var boundStatements = statement.Statements.Select(stat => Bind(stat)).ToArray();
            return new BoundDivideStatement(boundStatements);
        }
    }
}