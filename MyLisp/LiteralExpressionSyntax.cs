namespace MyLisp
{
    internal class LiteralExpressionSyntax : StatementSyntax
    {
        public LiteralExpressionSyntax(SyntaxToken literalToken)
            : this(literalToken, literalToken.Value)
        {
            Kind = SyntaxKind.LiteralExpression;
        }

        public LiteralExpressionSyntax(SyntaxToken literalToken, object value)
        {
            LiteralToken = literalToken;
            Value = value;
            Kind = SyntaxKind.LiteralExpression;
        }


        public SyntaxToken LiteralToken { get; }

        public object Value { get; }

    }
}