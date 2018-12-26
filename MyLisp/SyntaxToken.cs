namespace MyLisp
{
    public class SyntaxToken
    {
        private readonly SyntaxKind _kind;
        private readonly int _start;
        private readonly string _text;
        private readonly object _value;

        public SyntaxKind Kind => _kind;
        public SyntaxToken(SyntaxKind kind, int start, string text, object value)
        {
            _kind = kind;
            _start = start;
            _text = text;
            _value = value;
        }
    }
}