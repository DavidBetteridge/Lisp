namespace MyLisp
{
    internal class LiteralExpressionSyntax : StatementSyntax
    {
        private SyntaxToken numberToken;

        public LiteralExpressionSyntax(SyntaxToken numberToken)
        {
            this.numberToken = numberToken;
        }
    }
}