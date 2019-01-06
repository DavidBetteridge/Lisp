using System;
using System.Collections.Generic;

namespace MyLisp
{
    internal class MinusStatement : Statement
    {
        internal IEnumerable<Statement> BoundStatements { get; }

        public override NodeKind StatementNodeKind => NodeKind.MinusCommand;

        public MinusStatement(IEnumerable<Statement> boundStatements)
        {
            this.BoundStatements = boundStatements;
        }

    }
}