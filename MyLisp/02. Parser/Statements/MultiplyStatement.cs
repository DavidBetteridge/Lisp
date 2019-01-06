using System;
using System.Collections.Generic;

namespace MyLisp
{
    internal class MultiplyStatement : Statement
    {
        internal IEnumerable<Statement> BoundStatements { get; }

        public override NodeKind StatementNodeKind => NodeKind.MultiplyCommand;

        public MultiplyStatement(IEnumerable<Statement> boundStatements)
        {
            this.BoundStatements = boundStatements;
        }

    }
}