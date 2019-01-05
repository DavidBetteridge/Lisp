namespace MyLisp
{
    public abstract class StatementSyntax
    {
        public abstract CommandKind Kind { get; }
    }
}