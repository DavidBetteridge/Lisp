using System;
using System.Collections.Generic;

namespace MyLisp
{
    internal class BoundDivideStatement : BoundStatement
    {
        private readonly Type _type;

        internal IEnumerable<BoundStatement> BoundStatements { get; }

        public override BoundNodeKind BoundNodeKind => BoundNodeKind.DivideCommand;

        public override Type Type => _type;

        public BoundDivideStatement(Type type, IEnumerable<BoundStatement> boundStatements)
        {
            _type = type;
            this.BoundStatements = boundStatements;
        }

    }
}