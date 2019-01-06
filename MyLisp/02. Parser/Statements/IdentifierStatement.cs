using System;

namespace MyLisp
{
    class IdentifierStatement : Statement
    {
        public IdentifierStatement(string variableName)
        {
            this.VariableName = variableName;
        }

        public override NodeKind StatementNodeKind => NodeKind.Identifier;

        public string VariableName { get; }
    }
}
