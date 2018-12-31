using System;
using System.Collections.Generic;

namespace MyLisp
{
    internal class BoundOnePlusStatement : BoundStatement
    {
        internal BoundStatement BoundStatement { get; }

        public override BoundNodeKind BoundNodeKind => BoundNodeKind.OnePlusCommand;

        public BoundOnePlusStatement(BoundStatement boundStatement)
        {
            this.BoundStatement = boundStatement;
        }

    }
}