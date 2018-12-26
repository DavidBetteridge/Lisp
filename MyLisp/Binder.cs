using System;
using System.Linq;

namespace MyLisp
{
    internal class Binder
    {
        internal BoundStatement Bind(StatementSyntax statement)
        {
            switch (statement.Kind)
            {
                case SyntaxKind.PlusCommand:
                    return BindPlusStatement((PlusStatementSyntax)statement);

                case SyntaxKind.LiteralExpression:
                    return BindLiteralExpression((LiteralExpressionSyntax)statement);

                default:
                    throw new Exception($"Unexpected syntax {statement.Kind}");
            }
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
    }
}