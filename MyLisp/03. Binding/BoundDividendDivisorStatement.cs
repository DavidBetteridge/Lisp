using System;
using System.Collections.Generic;

namespace MyLisp
{
    internal class BoundDividendDivisorStatement : BoundStatement
    {
        internal BoundStatement BoundDividendStatement { get; }
        internal BoundStatement BoundDivisorStatement { get; }

        public override BoundNodeKind BoundNodeKind => BoundNodeKind.DividendDivisorCommand;

        public BoundDividendDivisorStatement(BoundStatement boundDividendStatement, BoundStatement boundDivisorStatement)
        {
            this.BoundDividendStatement = boundDividendStatement;
            this.BoundDivisorStatement = boundDivisorStatement;
        }

    }
}