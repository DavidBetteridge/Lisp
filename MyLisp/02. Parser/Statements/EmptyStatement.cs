namespace MyLisp
{
    internal class EmptyStatement : Statement
    {
        public override NodeKind StatementNodeKind => NodeKind.Empty;
    }
}