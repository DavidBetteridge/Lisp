using System;
using System.Collections.Generic;

namespace MyLisp
{
    internal class OnePlusStatement : Statement
    {
        internal Statement BoundStatement { get; }

        public override NodeKind StatementNodeKind => NodeKind.OnePlusCommand;

        public OnePlusStatement(Statement boundStatement)
        {
            this.BoundStatement = boundStatement;
        }

    }
}