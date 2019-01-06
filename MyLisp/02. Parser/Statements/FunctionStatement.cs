using System.Collections.Generic;

namespace MyLisp
{
    class FunctionStatement : Statement
    {
        public FunctionStatement(string functionName, IEnumerable<string> parameters, Statement body)
        {
            this.FunctionName = functionName;
            this.Parameters = parameters;
            this.Body = body;
        }

        public override NodeKind StatementNodeKind => NodeKind.DefFunCommand;

        public string FunctionName { get; set; }
        public IEnumerable<string> Parameters { get; set; }
        public Statement Body { get; set; }
    }
}
