namespace MyLisp
{
    public class SyntaxToken
    {
        private readonly string _text;

        public SyntaxKind Kind { get; }
        public int Position { get; }

        public object Value { get; }

        public SyntaxToken(SyntaxKind kind, int position, string text, object value)
        {
            Kind = kind;
            Position = position;
            _text = text;
            Value = value;
        }
    }
}