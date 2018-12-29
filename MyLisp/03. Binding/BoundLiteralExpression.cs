using System;

namespace MyLisp
{
    internal class BoundLiteralExpression : BoundStatement
    {
        public override BoundNodeKind BoundNodeKind => BoundNodeKind.Literal;

        public override Type Type => Value.GetType();

        public object Value { get; set; }

        public BoundLiteralExpression(object value)
        {
            Value = value;
        }
    }
}