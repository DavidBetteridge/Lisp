using System;
using System.Collections.Generic;

namespace MyLisp
{
    internal class BoundPlusStatement : BoundStatement
    {
        internal IEnumerable<BoundStatement> BoundStatements { get; }

        public override BoundNodeKind BoundNodeKind => BoundNodeKind.PlusCommand;

        public BoundPlusStatement(IEnumerable<BoundStatement> boundStatements)
        {
            this.BoundStatements = boundStatements;
        }

    }
}