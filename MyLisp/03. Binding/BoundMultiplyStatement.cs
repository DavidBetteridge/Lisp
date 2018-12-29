using System;
using System.Collections.Generic;

namespace MyLisp
{
    internal class BoundMultiplyStatement : BoundStatement
    {
        private readonly Type _type;

        internal IEnumerable<BoundStatement> BoundStatements { get; }

        public override BoundNodeKind BoundNodeKind => BoundNodeKind.MultiplyCommand;

        public override Type Type => _type;

        public BoundMultiplyStatement(Type type, IEnumerable<BoundStatement> boundStatements)
        {
            _type = type;
            this.BoundStatements = boundStatements;
        }

    }
}