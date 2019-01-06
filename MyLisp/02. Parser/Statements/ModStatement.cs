using System;
using System.Collections.Generic;

namespace MyLisp
{
    internal class ModStatement : Statement
    {
        internal Statement BoundDividendStatement { get; }
        internal Statement BoundDivisorStatement { get; }

        public override NodeKind StatementNodeKind => NodeKind.ModCommand;

        public ModStatement(Statement boundDividendStatement, Statement boundDivisorStatement)
        {
            this.BoundDividendStatement = boundDividendStatement;
            this.BoundDivisorStatement = boundDivisorStatement;
        }

    }
}