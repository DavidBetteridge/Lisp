using System.Collections.Generic;

namespace MyLisp
{
    internal class FunctionCallStatement : Statement
    {
        public FunctionCallStatement(string functionName, IEnumerable<Statement> args)
        {
            this.FunctionName = functionName;
            this.Args = args;
        }

        public override NodeKind StatementNodeKind => NodeKind.FunctionCall;

        public string FunctionName { get; }
        public IEnumerable<Statement> Args { get;}
    }
}