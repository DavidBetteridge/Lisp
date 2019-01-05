namespace MyLisp
{
    internal class StringSyntax : StatementSyntax
    {
        private SyntaxToken token;

        public string Text => token.Text;
        public StringSyntax(SyntaxToken token)
        {
            this.token = token;
        }

        public override CommandKind Kind => CommandKind.String;
    }
}