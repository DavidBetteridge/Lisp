namespace MyLisp
{
    public abstract class StatementSyntax
    {
        public SyntaxKind Kind { get; protected set; }
    }
}