using System;

namespace MyLisp 
{
    public class StringStatement : Statement
    {
        public override NodeKind StatementNodeKind => NodeKind.String;

        public string Value { get; }

        public StringStatement(string value)
        {
            Value = value;
        }
    }
}
