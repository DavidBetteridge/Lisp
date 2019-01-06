namespace MyLisp
{
    internal class IfStatement : Statement
    {
        public IfStatement(Statement predicate, Statement trueBranch, Statement falseBranch)
        {
            Predicate = predicate;
            TrueBranch = trueBranch;
            FalseBranch = falseBranch;
        }

        public override NodeKind StatementNodeKind => NodeKind.IfCommand;

        public Statement Predicate { get; }

        public Statement TrueBranch { get; }

        public Statement FalseBranch { get; }
    }
}