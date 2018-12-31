using System;
using System.Collections.Generic;

namespace MyLisp
{
    internal class BoundModStatement : BoundStatement
    {
        internal BoundStatement BoundDividendStatement { get; }
        internal BoundStatement BoundDivisorStatement { get; }

        public override BoundNodeKind BoundNodeKind => BoundNodeKind.ModCommand;

        public BoundModStatement(BoundStatement boundDividendStatement, BoundStatement boundDivisorStatement)
        {
            this.BoundDividendStatement = boundDividendStatement;
            this.BoundDivisorStatement = boundDivisorStatement;
        }

    }
}