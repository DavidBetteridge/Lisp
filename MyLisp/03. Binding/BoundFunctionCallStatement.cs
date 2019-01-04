using System.Collections.Generic;

namespace MyLisp
{
    internal class BoundFunctionCallStatement : BoundStatement
    {
        public BoundFunctionCallStatement(string functionName, IEnumerable<BoundStatement> args)
        {
            this.FunctionName = functionName;
            this.Args = args;
        }

        public override BoundNodeKind BoundNodeKind => BoundNodeKind.FunctionCall;

        public string FunctionName { get; }
        public IEnumerable<BoundStatement> Args { get;}
    }
}