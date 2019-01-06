using System;
using System.Collections.Generic;

namespace MyLisp
{
    internal class DivideStatement : Statement
    {
        internal IEnumerable<Statement> BoundStatements { get; }

        public override NodeKind StatementNodeKind => NodeKind.DivideCommand;

        public DivideStatement(IEnumerable<Statement> boundStatements)
        {
            this.BoundStatements = boundStatements;
        }

    }
}