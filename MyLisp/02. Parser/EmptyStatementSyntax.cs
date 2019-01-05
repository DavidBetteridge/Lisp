namespace MyLisp
{
    internal class EmptyStatementSyntax : StatementSyntax
    {
        private SyntaxToken openToken;
        private SyntaxToken endToken;

        public EmptyStatementSyntax(SyntaxToken openToken, SyntaxToken endToken)
        {
            this.openToken = openToken;
            this.endToken = endToken;
        }

        public override CommandKind Kind => CommandKind.Empty;
    }
}