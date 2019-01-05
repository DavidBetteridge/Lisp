using System.Collections.Generic;

namespace MyLisp
{
    class BoundFunctionStatement : BoundStatement
    {
        public BoundFunctionStatement(string functionName, IEnumerable<string> parameters, BoundStatement body)
        {
            this.FunctionName = functionName;
            this.Parameters = parameters;
            this.Body = body;
        }

        public override BoundNodeKind BoundNodeKind => BoundNodeKind.DefFunCommand;

        public string FunctionName { get; set; }
        public IEnumerable<string> Parameters { get; set; }
        public BoundStatement Body { get; set; }
    }
}
