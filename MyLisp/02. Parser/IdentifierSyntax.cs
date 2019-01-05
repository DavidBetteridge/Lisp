namespace MyLisp
{
    internal class IdentifierSyntax : StatementSyntax
    {
        public IdentifierSyntax(SyntaxToken token)
            : this(token, token.Value)
        {
        }

        public IdentifierSyntax(SyntaxToken token, object value)
        {
            LiteralToken = token;
            Value = value;
        }


        public SyntaxToken LiteralToken { get; }

        public object Value { get; }

        public override CommandKind Kind => CommandKind.IdentifierExpression;
    }
}