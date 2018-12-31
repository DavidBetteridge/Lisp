using System;
using System.Collections.Generic;

namespace MyLisp
{
    internal class BoundMultiplyStatement : BoundStatement
    {
        internal IEnumerable<BoundStatement> BoundStatements { get; }

        public override BoundNodeKind BoundNodeKind => BoundNodeKind.MultiplyCommand;

        public BoundMultiplyStatement(IEnumerable<BoundStatement> boundStatements)
        {
            this.BoundStatements = boundStatements;
        }

    }
}