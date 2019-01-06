using System;
using System.Collections.Generic;

namespace MyLisp
{
    internal class DividendDivisorStatement : Statement
    {
        internal Statement BoundDividendStatement { get; }
        internal Statement BoundDivisorStatement { get; }

        public override NodeKind StatementNodeKind => NodeKind.DividendDivisorCommand;

        public DividendDivisorStatement(Statement boundDividendStatement, Statement boundDivisorStatement)
        {
            this.BoundDividendStatement = boundDividendStatement;
            this.BoundDivisorStatement = boundDivisorStatement;
        }

    }
}