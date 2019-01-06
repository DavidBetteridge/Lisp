using System;
using System.Collections.Generic;

namespace MyLisp
{
    internal class OneMinusStatement : Statement
    {
        internal Statement BoundStatement { get; }

        public override NodeKind StatementNodeKind => NodeKind.OneMinusCommand;

        public OneMinusStatement(Statement boundStatement)
        {
            this.BoundStatement = boundStatement;
        }

    }
}