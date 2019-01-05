namespace MyLisp
{
    public class SyntaxToken
    {
        public SyntaxKind Kind { get; }
        public int Position { get; }

        public object Value { get; }

        public SyntaxToken(SyntaxKind kind, int position, string text, object value)
        {
            Kind = kind;
            Position = position;
            Text = text;
            Value = value;
        }

        public TextSpan Span => new TextSpan(Position, Text.Length);

        public string Text { get; }
    }
}