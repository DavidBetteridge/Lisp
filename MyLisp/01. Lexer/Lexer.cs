namespace MyLisp
{
    class Lexer
    {
        private readonly DiagnosticBag _diagnostics = new DiagnosticBag();
        private readonly string _text;  //Use span
        private int _position;
        private int _start;
        private SyntaxKind _kind;

        private object _value;
        private char Current => Peek(0);
        private char Lookahead => Peek(1);
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
            _start = _position;
            _kind = SyntaxKind.BadToken;
            _value = null;

            switch (Current)
            {
                case '1' when Lookahead == '+':
                    _kind = SyntaxKind.OnePlusToken;
                    _position += 2;
                    break;

                case '1' when Lookahead == '-':
                    _kind = SyntaxKind.OneMinusToken;
                    _position += 2;
                    break;

                case '\0':
                    _kind = SyntaxKind.EndOfFileToken;
                    break;

                case '+':
                    _kind = SyntaxKind.PlusToken;
                    _position++;
                    break;

                case '-' when Lookahead == ' ' || Lookahead == ')':
                    _kind = SyntaxKind.MinusToken;
                    _position++;
                    break;

                case '*':
                    _kind = SyntaxKind.StarToken;
                    _position++;
                    break;

                case '/':
                    _kind = SyntaxKind.SlashToken;
                    _position++;
                    break;

                case '(':
                    _kind = SyntaxKind.OpenParenthesisToken;
                    _position++;
                    break;

                case ')':
                    _kind = SyntaxKind.CloseParenthesisToken;
                    _position++;
                    break;

                case '-':
                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                    ReadNumber();
                    break;
                case ' ':
                case '\t':
                case '\n':
                case '\r':
                    ReadWhiteSpace();
                    break;

                default:

                    if (char.IsLetter(Current))
                    {
                        ReadIdentifierOrKeyword();
                    }
                    else if (char.IsWhiteSpace(Current))
                    {
                        ReadWhiteSpace();
                    }
                    else
                    {
                        _diagnostics.ReportBadCharacter(_position, Current);
                        _position++;
                    }
                    break;
            }

            var length = _position - _start;
            var text = SyntaxFacts.GetText(_kind);
            if (text == null)
                text = _text.Substring(_start, length);

            return new SyntaxToken(_kind, _start, text, _value);
        }

        private void ReadNumber()
        {
            //TODO:  Refactor this so that it keeps eating tokens until it hits either whitespace or )
            if (Current == '-')
                _position++;

            while (char.IsDigit(Current) || Current == '.')
                _position++;

            var length = _position - _start;
            var text = _text.Substring(_start, length);

            if (int.TryParse(text, out var integerValue))
            {
                _value = integerValue;
                _kind = SyntaxKind.IntegerNumberToken;
            }
            else if (double.TryParse(text, out var doubleValue))
            {
                _value = doubleValue;
                _kind = SyntaxKind.FloatingPointNumberToken;
            }
            else
            {
                _diagnostics.ReportInvalidNumber(new TextSpan(_start, length), text, typeof(int));
            }

        }

        private void ReadWhiteSpace()
        {
            while (char.IsWhiteSpace(Current))
                _position++;

            _kind = SyntaxKind.WhitespaceToken;
        }


        private void ReadIdentifierOrKeyword()
        {
            while (char.IsLetter(Current))
                _position++;

            var length = _position - _start;
            var text = _text.Substring(_start, length);
            _kind = SyntaxFacts.GetKeywordKind(text);
        }
    }

}
