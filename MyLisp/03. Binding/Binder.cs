using System;
using System.Linq;

namespace MyLisp
{
    public class Binder
    {
        private Environment _environment;

        public Binder(Environment environment)
        {
            _environment = environment;
        }
        public BoundStatement Bind(StatementSyntax statement)
        {
            switch (statement.Kind)
            {
                case SyntaxKind.OnePlusCommand:
                    return BindOnePlusStatement((CommandStatementSyntax)statement);

                case SyntaxKind.OneMinusCommand:
                    return BindOneMinusStatement((CommandStatementSyntax)statement);

                case SyntaxKind.PlusCommand:
                    return BindPlusStatement((CommandStatementSyntax)statement);

                case SyntaxKind.MinusCommand:
                    return BindMinusStatement((CommandStatementSyntax)statement);

                case SyntaxKind.MultiplyCommand:
                    return BindMultiplyStatement((CommandStatementSyntax)statement);

                case SyntaxKind.DivideCommand:
                    return BindDivideStatement((CommandStatementSyntax)statement);

                case SyntaxKind.DividendDivisorCommand:
                    return BindDividendDivisorStatement((CommandStatementSyntax)statement);

                case SyntaxKind.DefVarCommand:
                    return BindDefVarStatement((CommandStatementSyntax)statement);

                case SyntaxKind.ModCommand:
                    return BindModStatement((CommandStatementSyntax)statement);

                case SyntaxKind.LiteralExpression:
                    return BindLiteralExpression((LiteralExpressionSyntax)statement);

                case SyntaxKind.IdentifierExpression:
                    return BindIdentifier((IdentifierSyntax)statement);

                default:
                    throw new Exception($"Unexpected syntax {statement.Kind}");
            }
        }

        private BoundIdentifierStatement BindIdentifier(IdentifierSyntax statement)
        {
            var variableName = (string)statement.Value;
            if (!_environment.IsDefined(variableName))
                throw new Exception($"The variable {variableName} has not been defined.");

            //var type = _environment.ReadType(variableName);
            var type = typeof(int);

            return new BoundIdentifierStatement(variableName, type);
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

            _environment.Define(name, null, documentation);

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