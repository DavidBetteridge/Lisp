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

                case SyntaxKind.LiteralExpression:
                    return BindLiteralExpression((LiteralExpressionSyntax)statement);

                default:
                    throw new Exception($"Unexpected syntax {statement.Kind}");
            }
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
            return new BoundOneMinusStatement(boundStatement.Type, boundStatement);
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
            return new BoundOnePlusStatement(boundStatement.Type, boundStatement);
        }

        private BoundLiteralExpression BindLiteralExpression(LiteralExpressionSyntax statement)
        {
            return new BoundLiteralExpression(statement.Value);
        }

        private BoundPlusStatement BindPlusStatement(CommandStatementSyntax statement)
        {
            var boundStatements = statement.Statements.Select(stat => Bind(stat));
            if (boundStatements.All(s => s.Type == typeof(int)))
                return new BoundPlusStatement(typeof(int), boundStatements);
            else
                return null;
        }

        private BoundMinusStatement BindMinusStatement(CommandStatementSyntax statement)
        {
            var boundStatements = statement.Statements.Select(stat => Bind(stat));
            if (boundStatements.All(s => s.Type == typeof(int)))
                return new BoundMinusStatement(typeof(int), boundStatements);
            else
                return null;
        }

        private BoundMultiplyStatement BindMultiplyStatement(CommandStatementSyntax statement)
        {
            var boundStatements = statement.Statements.Select(stat => Bind(stat));
            if (boundStatements.All(s => s.Type == typeof(int)))
                return new BoundMultiplyStatement(typeof(int), boundStatements);
            else
                return null;
        }

        private BoundDivideStatement BindDivideStatement(CommandStatementSyntax statement)
        {
            var boundStatements = statement.Statements.Select(stat => Bind(stat));
            if (boundStatements.All(s => s.Type == typeof(int)))
                return new BoundDivideStatement(typeof(int), boundStatements);
            else
                return new BoundDivideStatement(typeof(double), boundStatements);
        }
    }
}