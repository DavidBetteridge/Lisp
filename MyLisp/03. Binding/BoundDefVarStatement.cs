using System;

namespace MyLisp
{
    class BoundDefVarStatement : BoundStatement
    {
        public BoundStatement InitialValue { get; set; }
        public string Name { get; set; }
        public string Documentation { get; set; }
        public BoundDefVarStatement(string name, BoundStatement initialValue, string documentation)
        {
            this.Name = name;
            this.InitialValue = initialValue;
            this.Documentation = documentation;
        }

        public override BoundNodeKind BoundNodeKind => BoundNodeKind.DefVarCommand;

        public override Type Type => InitialValue.GetType();


    }
}
