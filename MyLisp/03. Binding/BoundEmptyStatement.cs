namespace MyLisp
{
    internal class BoundEmptyStatement : BoundStatement
    {
        public override BoundNodeKind BoundNodeKind => BoundNodeKind.Empty;
    }
}