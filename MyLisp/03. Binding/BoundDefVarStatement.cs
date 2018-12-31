using System;

namespace MyLisp
{
    class BoundDefVarStatement : BoundStatement
    {
        private string name;
        private object initialValue;
        private string documentation;

        public BoundDefVarStatement(string name, object initialValue, string documentation)
        {
            this.name = name;
            this.initialValue = initialValue;
            this.documentation = documentation;
        }

        public override BoundNodeKind BoundNodeKind => BoundNodeKind.DefVarCommand;

        public override Type Type => initialValue.GetType();
    }
}
