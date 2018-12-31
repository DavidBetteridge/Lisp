using System;
using System.Collections.Generic;

namespace MyLisp
{
    internal class BoundOneMinusStatement : BoundStatement
    {
        internal BoundStatement BoundStatement { get; }

        public override BoundNodeKind BoundNodeKind => BoundNodeKind.OneMinusCommand;

        public BoundOneMinusStatement(BoundStatement boundStatement)
        {
            this.BoundStatement = boundStatement;
        }

    }
}