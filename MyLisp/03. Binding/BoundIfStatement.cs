namespace MyLisp
{
    internal class BoundIfStatement : BoundStatement
    {
        public BoundIfStatement(BoundStatement predicate, BoundStatement trueBranch, BoundStatement falseBranch)
        {
            Predicate = predicate;
            TrueBranch = trueBranch;
            FalseBranch = falseBranch;
        }

        public override BoundNodeKind BoundNodeKind => BoundNodeKind.IfCommand;

        public BoundStatement Predicate { get; }

        public BoundStatement TrueBranch { get; }

        public BoundStatement FalseBranch { get; }
    }
}