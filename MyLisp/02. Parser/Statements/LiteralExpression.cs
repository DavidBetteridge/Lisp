using System;

namespace MyLisp
{
    internal class LiteralExpression : Statement
    {
        public override NodeKind StatementNodeKind => NodeKind.Literal;

        public object Value { get; set; }

        public LiteralExpression(object value)
        {
            Value = value;
        }
    }
}