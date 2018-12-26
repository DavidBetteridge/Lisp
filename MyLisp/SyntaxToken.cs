namespace MyLisp
{
    public class SyntaxToken
    {
        private readonly SyntaxKind _kind;
        private readonly int _position;
        private readonly string _text;
        private readonly object _value;

        public SyntaxKind Kind => _kind;
        public int Position => _position;
        public SyntaxToken(SyntaxKind kind, int position, string text, object value)
        {
            _kind = kind;
            _position = position;
            _text = text;
            _value = value;
        }
    }
}