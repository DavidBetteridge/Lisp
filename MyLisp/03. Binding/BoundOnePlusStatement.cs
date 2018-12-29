using System;
using System.Collections.Generic;

namespace MyLisp
{
    internal class BoundOnePlusStatement : BoundStatement
    {
        private readonly Type _type;

        internal BoundStatement BoundStatement { get; }

        public override BoundNodeKind BoundNodeKind => BoundNodeKind.OnePlusCommand;

        public override Type Type => _type;

        public BoundOnePlusStatement(Type type, BoundStatement boundStatement)
        {
            _type = type;
            this.BoundStatement = boundStatement;
        }

    }
}