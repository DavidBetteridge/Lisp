using System;
using System.Collections.Generic;

namespace MyLisp
{
    internal class BoundModStatement : BoundStatement
    {
        private readonly Type _type;

        internal BoundStatement BoundDividendStatement { get; }
        internal BoundStatement BoundDivisorStatement { get; }

        public override BoundNodeKind BoundNodeKind => BoundNodeKind.ModCommand;

        public override Type Type => _type;

        public BoundModStatement(Type type, BoundStatement boundDividendStatement, BoundStatement boundDivisorStatement)
        {
            _type = type;
            this.BoundDividendStatement = boundDividendStatement;
            this.BoundDivisorStatement = boundDivisorStatement;
        }

    }
}