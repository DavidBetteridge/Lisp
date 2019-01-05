namespace MyLisp
{
    public class Lexer
    {
        private readonly DiagnosticBag _diagnostics = new DiagnosticBag();
        private readonly string _text;
        private int _position;

        private char Current => Peek(0);

        private char Peek(int offset)
        {
            var index = _position + offset;
            if (index >= _text.Length)
                return '\0';

            return _text[index];
        }
        public Lexer(string text)
        {
            _text = text;
        }

        public SyntaxToken Lex()
        {
            SkipWhiteSpace();

            var start = _position;
            var kind = SyntaxKind.BadToken;
            object value = ReadNextToken();
            switch (value)
            {
                case "\0":
                    kind = SyntaxKind.EndOfFileToken;
                    break;

                case "(":
                    kind = SyntaxKind.OpenParenthesisToken;
                    break;

                case ")":
                    kind = SyntaxKind.CloseParenthesisToken;
                    break;

                case "+":
                    kind = SyntaxKind.PlusToken;
                    break;

                case "-":
                    kind = SyntaxKind.MinusToken;
                    break;

                case "/":
                    kind = SyntaxKind.SlashToken;
                    break;

                case "*":
                    kind = SyntaxKind.StarToken;
                    break;

                case "%":
                    kind = SyntaxKind.PercentToken;
                    break;

                case "'":
                    kind = SyntaxKind.QuoteToken;
                    break;

                case "1+":
                    kind = SyntaxKind.OnePlusToken;
                    break;

                case "1-":
                    kind = SyntaxKind.OneMinusToken;
                    break;

                default:
                    if (int.TryParse((string)value, out var i))
                    {
                        kind = SyntaxKind.IntegerNumberToken;
                        value = i;
                    }

                    else if (double.TryParse((string)value, out var d))
                    {
                        kind = SyntaxKind.FloatingPointNumberToken;
                        value = d;
                    }

                    else if (((string)value).StartsWith(@""""))
                        kind = SyntaxKind.StringToken;

                    else
                        kind = SyntaxKind.IdentifierToken;

                    break;
            }

            //Unterminated strings
            //Invalid identifiers 123...

            var textLength = _position - start;
            if (textLength + start > _text.Length) textLength--;  //exclude\0
            return new SyntaxToken(kind, start, _text.Substring(start, textLength), value);
        }

        private string ReadNextToken()
        {
            string value = Current.ToString();
            _position++;

            switch (value)
            {
                case "\0":
                    break;

                case "(":
                    break;

                case ")":
                    break;

                case "'":
                    break;

                case @"""":
                    while (Current != '"')
                    {
                        value += Current.ToString();
                        _position++;
                    }
                    value += Current.ToString();
                    _position++;
                    break;

                default:

                    while (Current != ' '
                            && Current != '('
                            && Current != ')'
                            && Current != '\''
                            && Current != '\0'
                            && Current != '"'
    )
                    {
                        value += Current.ToString();
                        _position++;
                    }
                    break;
            }

            return value;
        }

        void SkipWhiteSpace()
        {
            while (Current == ' ')
                _position++;
        }


    }

}
