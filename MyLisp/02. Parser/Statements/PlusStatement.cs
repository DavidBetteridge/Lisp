using System;
using System.Collections.Generic;

namespace MyLisp
{
    internal class PlusStatement : Statement
    {
        internal IEnumerable<Statement> BoundStatements { get; }

        public override NodeKind StatementNodeKind => NodeKind.PlusCommand;

        public PlusStatement(IEnumerable<Statement> boundStatements)
        {
            this.BoundStatements = boundStatements;
        }

    }
}