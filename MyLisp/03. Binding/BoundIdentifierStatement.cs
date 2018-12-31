using System;

namespace MyLisp
{
    class BoundIdentifierStatement : BoundStatement
    {
        private readonly Type _type;

        public BoundIdentifierStatement(string variableName, Type type)
        {
            this.VariableName = variableName;
            this._type = type;
        }

        public override BoundNodeKind BoundNodeKind => BoundNodeKind.Identifier;

        public string VariableName { get; }
    }
}
