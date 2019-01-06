using System;

namespace MyLisp
{
    class DefVarStatement : Statement
    {
        public Statement InitialValue { get; set; }
        public string Name { get; set; }
        public string Documentation { get; set; }
        public DefVarStatement(string name, Statement initialValue, string documentation)
        {
            this.Name = name;
            this.InitialValue = initialValue;
            this.Documentation = documentation;
        }

        public override NodeKind StatementNodeKind => NodeKind.DefVarCommand;

    }
}
