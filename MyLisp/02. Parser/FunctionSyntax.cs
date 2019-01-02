using System.Collections.Generic;

namespace MyLisp
{
    internal class FunctionSyntax : StatementSyntax
    {
        private SyntaxToken openToken;
        private SyntaxToken command;
        private SyntaxToken endToken;

        public FunctionSyntax(SyntaxToken openToken, 
                              SyntaxToken command, 
                              SyntaxToken functionName, 
                              List<SyntaxToken> parameters, 
                              StatementSyntax body, 
                              SyntaxToken endToken)
        {
            this.openToken = openToken;
            this.command = command;
            this.FunctionName = functionName;
            this.Parameters = parameters;
            this.Body = body;
            this.endToken = endToken;
        }

        public override SyntaxKind Kind => SyntaxKind.DefFunCommand;

        public SyntaxToken FunctionName { get; set; }
        public List<SyntaxToken> Parameters { get; set; }
        public StatementSyntax Body { get; set; }
    }
}