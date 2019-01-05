using System;

namespace MyLisp
{
    class BoundIdentifierStatement : BoundStatement
    {
        public BoundIdentifierStatement(string variableName)
        {
            this.VariableName = variableName;
        }

        public override BoundNodeKind BoundNodeKind => BoundNodeKind.Identifier;

        public string VariableName { get; }
    }
}
