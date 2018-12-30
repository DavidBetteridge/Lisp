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
                    return BindOnePlusStatement((OnePlusStatementSyntax)statement);

                case SyntaxKind.OneMinusCommand:
                    return BindOneMinusStatement((OneMinusStatementSyntax)statement);

                case SyntaxKind.PlusCommand:
                    return BindPlusStatement((PlusStatementSyntax)statement);

                case SyntaxKind.MinusCommand:
                    return BindMinusStatement((MinusStatementSyntax)statement);

                case SyntaxKind.MultiplyCommand:
                    return BindMultiplyStatement((MultiplyStatementSyntax)statement);

                case SyntaxKind.DivideCommand:
                    return BindDivideStatement((DivideStatementSyntax)statement);

                case SyntaxKind.LiteralExpression:
                    return BindLiteralExpression((LiteralExpressionSyntax)statement);

                default:
                    throw new Exception($"Unexpected syntax {statement.Kind}");
            }
        }

        private BoundOneMinusStatement BindOneMinusStatement(OneMinusStatementSyntax statement)
        {
            var boundStatement = Bind(statement.Statement);
            return new BoundOneMinusStatement(boundStatement.Type, boundStatement);
        }

        private BoundOnePlusStatement BindOnePlusStatement(OnePlusStatementSyntax statement)
        {
            var boundStatement = Bind(statement.Statement);
            return new BoundOnePlusStatement(boundStatement.Type, boundStatement);
        }

        private BoundLiteralExpression BindLiteralExpression(LiteralExpressionSyntax statement)
        {
            return new BoundLiteralExpression(statement.Value);
        }

        private BoundPlusStatement BindPlusStatement(PlusStatementSyntax statement)
        {
            var boundStatements = statement.Statements.Select(stat => Bind(stat));
            if (boundStatements.All(s => s.Type == typeof(int)))
                return new BoundPlusStatement(typeof(int), boundStatements);
            else
                return null;
        }

        private BoundMinusStatement BindMinusStatement(MinusStatementSyntax statement)
        {
            var boundStatements = statement.Statements.Select(stat => Bind(stat));
            if (boundStatements.All(s => s.Type == typeof(int)))
                return new BoundMinusStatement(typeof(int), boundStatements);
            else
                return null;
        }

        private BoundMultiplyStatement BindMultiplyStatement(MultiplyStatementSyntax statement)
        {
            var boundStatements = statement.Statements.Select(stat => Bind(stat));
            if (boundStatements.All(s => s.Type == typeof(int)))
                return new BoundMultiplyStatement(typeof(int), boundStatements);
            else
                return null;
        }

        private BoundDivideStatement BindDivideStatement(DivideStatementSyntax statement)
        {
            var boundStatements = statement.Statements.Select(stat => Bind(stat));
            if (boundStatements.All(s => s.Type == typeof(int)))
                return new BoundDivideStatement(typeof(int), boundStatements);
            else
                return new BoundDivideStatement(typeof(double), boundStatements);
        }
    }
}