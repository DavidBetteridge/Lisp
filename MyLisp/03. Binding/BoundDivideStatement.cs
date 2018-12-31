using System;
using System.Collections.Generic;

namespace MyLisp
{
    internal class BoundDivideStatement : BoundStatement
    {
        internal IEnumerable<BoundStatement> BoundStatements { get; }

        public override BoundNodeKind BoundNodeKind => BoundNodeKind.DivideCommand;

        public BoundDivideStatement(IEnumerable<BoundStatement> boundStatements)
        {
            this.BoundStatements = boundStatements;
        }

    }
}