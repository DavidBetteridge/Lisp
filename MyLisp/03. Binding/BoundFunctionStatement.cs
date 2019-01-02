﻿using System.Collections.Generic;

namespace MyLisp
{
    class BoundFunctionStatement : BoundStatement
    {
        public BoundFunctionStatement(SyntaxToken functionName, List<SyntaxToken> parameters, StatementSyntax body)
        {
            this.FunctionName = functionName;
            this.Parameters = parameters;
            this.Body = body;
        }

        public override BoundNodeKind BoundNodeKind => BoundNodeKind.DefFunCommand;

        public SyntaxToken FunctionName { get; set; }
        public List<SyntaxToken> Parameters { get; set; }
        public StatementSyntax Body { get; set; }
    }
}
