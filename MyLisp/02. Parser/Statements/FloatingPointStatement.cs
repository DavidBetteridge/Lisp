using System;

namespace MyLisp 
{
    public class FloatingPointStatement : Statement
    {
        public override NodeKind StatementNodeKind => NodeKind.FloatingPoint;

        public double Value { get; }

        public FloatingPointStatement(double value)
        {
            Value = value;
        }
    }
}
