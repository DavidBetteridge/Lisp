using System;
using System.Collections.Generic;

namespace MyLisp
{
    internal class BoundPlusStatement : BoundStatement
    {
        private readonly Type _type;

        internal IEnumerable<BoundStatement> BoundStatements { get; }

        public override BoundNodeKind BoundNodeKind => BoundNodeKind.PlusCommand;

        public override Type Type => _type;

        public BoundPlusStatement(Type type, IEnumerable<BoundStatement> boundStatements)
        {
            _type = type;
            this.BoundStatements = boundStatements;
        }

    }
}