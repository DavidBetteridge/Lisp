namespace MyLisp
{
    internal class LiteralExpressionSyntax : StatementSyntax
    {
        public LiteralExpressionSyntax(SyntaxToken literalToken)
            : this(literalToken, literalToken.Value)
        {
        }

        public LiteralExpressionSyntax(SyntaxToken literalToken, object value)
        {
            LiteralToken = literalToken;
            Value = value;
        }


        public SyntaxToken LiteralToken { get; }

        public object Value { get; }

        public override CommandKind Kind => CommandKind.LiteralExpression;
    }
}