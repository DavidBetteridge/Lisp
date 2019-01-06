using System;

namespace MyLisp 
{
    public class NumericStatement : Statement
    {
        public override NodeKind StatementNodeKind => NodeKind.Integer;

        public int Value { get; }

        public NumericStatement(int value)
        {
            Value = value;
        }
    }
}
