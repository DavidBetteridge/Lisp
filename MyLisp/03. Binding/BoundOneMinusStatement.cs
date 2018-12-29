using System;
using System.Collections.Generic;

namespace MyLisp
{
    internal class BoundOneMinusStatement : BoundStatement
    {
        private readonly Type _type;

        internal BoundStatement BoundStatement { get; }

        public override BoundNodeKind BoundNodeKind => BoundNodeKind.OneMinusCommand;

        public override Type Type => _type;

        public BoundOneMinusStatement(Type type, BoundStatement boundStatement)
        {
            _type = type;
            this.BoundStatement = boundStatement;
        }

    }
}