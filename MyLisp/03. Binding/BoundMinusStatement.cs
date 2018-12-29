using System;
using System.Collections.Generic;

namespace MyLisp
{
    internal class BoundMinusStatement : BoundStatement
    {
        private readonly Type _type;

        internal IEnumerable<BoundStatement> BoundStatements { get; }

        public override BoundNodeKind BoundNodeKind => BoundNodeKind.MinusCommand;

        public override Type Type => _type;

        public BoundMinusStatement(Type type, IEnumerable<BoundStatement> boundStatements)
        {
            _type = type;
            this.BoundStatements = boundStatements;
        }

    }
}