using System;
using System.Linq;

namespace MyLisp
{
    public class Binder
    {
        public BoundStatement Bind(StatementSyntax statement)
        {
            switch (statement.Kind)
            {
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

        private BoundIdentifierStatement BindIdentifier(IdentifierSyntax statement)
        {
            var variableName = (string)statement.Value;
            //if (!_environment.IsDefined(variableName))
            //    throw new Exception($"The variable {variableName} has not been defined.");

            //var type = _environment.ReadType(variableName);
            var type = typeof(int);

            return new BoundIdentifierStatement(variableName, type);
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

                default:
                    var args = statement.Statements.Select(Bind);
                    return new BoundFunctionCallStatement(functionName, args);
            }
        }

        private BoundFunctionStatement BindDefFunStatement(CommandStatementSyntax statement)
        {
            var first = statement.Statements[0] as IdentifierSyntax;
            var functionName = first.Value.ToString();

            var args = statement.Statements[1] as CommandStatementSyntax;
            var firstArgument =new string[] { args.Command.Value.ToString() };
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

            string documentation = "";
            //if (statement.Statements.Count() > 2)
            //    documentation = statement.Statements[2];

            //_environment.Define(name, null, documentation);

            return new BoundDefVarStatement(name, initialValue, documentation);
        }

        private BoundOneMinusStatement BindOneMinusStatement(CommandStatementSyntax statement)
        {
            var statements = statement.Statements;

            //switch (statements.Count())
            //{
            //    case 0:
            //        _diagnostics.ReportTooFewArguments(Current.Span, Current.Kind, "1-");
            //        break;
            //    case 1:
            //        //All good
            //        break;
            //    default:
            //        _diagnostics.ReportTooManyArguments(Current.Span, Current.Kind, "1-");
            //        break;
            //}

            var boundStatement = Bind(statements.First());
            return new BoundOneMinusStatement(boundStatement);
        }

        private BoundOnePlusStatement BindOnePlusStatement(CommandStatementSyntax statement)
        {
            var statements = statement.Statements;

            //switch (statements.Count())
            //{
            //    case 0:
            //        _diagnostics.ReportTooFewArguments(Current.Span, Current.Kind, "1+");
            //        break;
            //    case 1:
            //        //All good
            //        break;
            //    default:
            //        _diagnostics.ReportTooManyArguments(Current.Span, Current.Kind, "1+");
            //        break;
            //}

            var boundStatement = Bind(statements.First());
            return new BoundOnePlusStatement(boundStatement);
        }

        private BoundLiteralExpression BindLiteralExpression(LiteralExpressionSyntax statement)
        {
            return new BoundLiteralExpression(statement.Value);
        }

        private BoundPlusStatement BindPlusStatement(CommandStatementSyntax statement)
        {
            var boundStatements = statement.Statements.Select(stat => Bind(stat));
            return new BoundPlusStatement(boundStatements);
        }

        private BoundMinusStatement BindMinusStatement(CommandStatementSyntax statement)
        {
            var boundStatements = statement.Statements.Select(stat => Bind(stat));
            return new BoundMinusStatement(boundStatements);
        }

        private BoundMultiplyStatement BindMultiplyStatement(CommandStatementSyntax statement)
        {
            var boundStatements = statement.Statements.Select(stat => Bind(stat));
            return new BoundMultiplyStatement(boundStatements);
        }

        private BoundDividendDivisorStatement BindDividendDivisorStatement(CommandStatementSyntax statement)
        {
            //switch (statements.Count())
            //{
            //    case 0:
            //    case 1:
            //        _diagnostics.ReportTooFewArguments(Current.Span, Current.Kind, "%");
            //        break;
            //    case 2:
            //        //All good
            //        break;
            //    default:
            //        _diagnostics.ReportTooManyArguments(Current.Span, Current.Kind, "%");
            //        break;
            //}
            var boundStatements = statement.Statements.Select(stat => Bind(stat)).ToArray();
            var lhs = boundStatements[0];
            var rhs = boundStatements[1];
            return new BoundDividendDivisorStatement(lhs, rhs);
        }


        private BoundModStatement BindModStatement(CommandStatementSyntax statement)
        {
            //switch (statements.Count())
            //{
            //    case 0:
            //    case 1:
            //        _diagnostics.ReportTooFewArguments(Current.Span, Current.Kind, "%");
            //        break;
            //    case 2:
            //        //All good
            //        break;
            //    default:
            //        _diagnostics.ReportTooManyArguments(Current.Span, Current.Kind, "%");
            //        break;
            //}
            var boundStatements = statement.Statements.Select(stat => Bind(stat)).ToArray();
            var lhs = boundStatements[0];
            var rhs = boundStatements[1];

            return new BoundModStatement(lhs, rhs);
        }

        private BoundDivideStatement BindDivideStatement(CommandStatementSyntax statement)
        {
            var boundStatements = statement.Statements.Select(stat => Bind(stat));
            return new BoundDivideStatement(boundStatements);
        }
    }
}