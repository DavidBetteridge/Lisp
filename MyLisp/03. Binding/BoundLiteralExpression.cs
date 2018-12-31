using System;

namespace MyLisp
{
    internal class BoundLiteralExpression : BoundStatement
    {
        public override BoundNodeKind BoundNodeKind => BoundNodeKind.Literal;

        public object Value { get; set; }

        public BoundLiteralExpression(object value)
        {
            Value = value;
        }
    }
}