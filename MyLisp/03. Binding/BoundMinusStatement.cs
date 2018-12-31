using System;
using System.Collections.Generic;

namespace MyLisp
{
    internal class BoundMinusStatement : BoundStatement
    {
        internal IEnumerable<BoundStatement> BoundStatements { get; }

        public override BoundNodeKind BoundNodeKind => BoundNodeKind.MinusCommand;

        public BoundMinusStatement(IEnumerable<BoundStatement> boundStatements)
        {
            this.BoundStatements = boundStatements;
        }

    }
}