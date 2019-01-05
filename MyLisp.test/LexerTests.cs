using System.Linq;
using Xunit;

namespace MyLisp.test
{
    public class LexerTests
    {
        [Fact]
        public void CanLexSymbols()
        {
            var sourceText = @"(+ (/ 1 -2 -3.34) ""hello"" '(a b c) )";
            var lexer = new Lexer(sourceText);

            Assert.Equal(SyntaxKind.OpenParenthesisToken, lexer.Lex().Kind);

            var token = lexer.Lex();
            Assert.Equal(SyntaxKind.PlusToken, token.Kind);
            Assert.Equal("+", token.Value);
            Assert.Equal("+", token.Text);

            Assert.Equal(SyntaxKind.OpenParenthesisToken, lexer.Lex().Kind);
            Assert.Equal(SyntaxKind.SlashToken, lexer.Lex().Kind);
            Assert.Equal(SyntaxKind.IntegerNumberToken, lexer.Lex().Kind);
            Assert.Equal(SyntaxKind.IntegerNumberToken, lexer.Lex().Kind);
            Assert.Equal(SyntaxKind.FloatingPointNumberToken, lexer.Lex().Kind);
            Assert.Equal(SyntaxKind.CloseParenthesisToken, lexer.Lex().Kind);

            token = lexer.Lex();
            Assert.Equal(SyntaxKind.StringToken, token.Kind);
            Assert.Equal(@"""hello""", token.Text);

            Assert.Equal(SyntaxKind.QuoteToken, lexer.Lex().Kind);
            Assert.Equal(SyntaxKind.OpenParenthesisToken, lexer.Lex().Kind);
            Assert.Equal(SyntaxKind.IdentifierToken, lexer.Lex().Kind);
            Assert.Equal(SyntaxKind.IdentifierToken, lexer.Lex().Kind);
            Assert.Equal(SyntaxKind.IdentifierToken, lexer.Lex().Kind);
            Assert.Equal(SyntaxKind.CloseParenthesisToken, lexer.Lex().Kind);
            Assert.Equal(SyntaxKind.CloseParenthesisToken, lexer.Lex().Kind);
        }

        [Fact]
        public void CanLexOperators()
        {
            var sourceText = @"+ - / * % 1+ 1-";
            var lexer = new Lexer(sourceText);

            Assert.Equal(SyntaxKind.PlusToken, lexer.Lex().Kind);
            Assert.Equal(SyntaxKind.MinusToken, lexer.Lex().Kind);
            Assert.Equal(SyntaxKind.SlashToken, lexer.Lex().Kind);
            Assert.Equal(SyntaxKind.StarToken, lexer.Lex().Kind);
            Assert.Equal(SyntaxKind.PercentToken, lexer.Lex().Kind);
            Assert.Equal(SyntaxKind.OnePlusToken, lexer.Lex().Kind);
            Assert.Equal(SyntaxKind.OneMinusToken, lexer.Lex().Kind);
            Assert.Equal(SyntaxKind.EndOfFileToken, lexer.Lex().Kind);
        }

        [Fact]
        public void CanLexModFunctionCalls()
        {
            var sourceText = @"(mod 1 2)";
            var lexer = new Lexer(sourceText);

            Assert.Equal(SyntaxKind.OpenParenthesisToken, lexer.Lex().Kind);

            var token = lexer.Lex();
            Assert.Equal(SyntaxKind.IdentifierToken, token.Kind);
            Assert.Equal("mod", token.Value);

            Assert.Equal(SyntaxKind.IntegerNumberToken, lexer.Lex().Kind);
            Assert.Equal(SyntaxKind.IntegerNumberToken, lexer.Lex().Kind);
            Assert.Equal(SyntaxKind.CloseParenthesisToken, lexer.Lex().Kind);
            Assert.Equal(SyntaxKind.EndOfFileToken, lexer.Lex().Kind);
        }

        [Fact]
        public void ReportErrorsForUnterminatedStrings()
        {
            var sourceText = @"(length ""this is a string)";
            var lexer = new Lexer(sourceText);

            Assert.Equal(SyntaxKind.OpenParenthesisToken, lexer.Lex().Kind);
            Assert.Equal(SyntaxKind.IdentifierToken, lexer.Lex().Kind);
            Assert.Equal(SyntaxKind.StringToken, lexer.Lex().Kind);

            var errors = lexer.DiagnosticBag.Errors;
            Assert.Single(errors);
            Assert.Equal(@"The string '""this is a string)' is not terminated", errors.First().Message);
            Assert.Equal(SyntaxKind.EndOfFileToken, lexer.Lex().Kind);
        }

        [Fact]
        public void AllowStringsToContainEscapedDoubleQuotes()
        {
            var sourceText = @"(length ""hello \""world"")";
            var lexer = new Lexer(sourceText);

            Assert.Equal(SyntaxKind.OpenParenthesisToken, lexer.Lex().Kind);
            Assert.Equal(SyntaxKind.IdentifierToken, lexer.Lex().Kind);

            var token = lexer.Lex();
            Assert.Equal(SyntaxKind.StringToken, token.Kind);
            Assert.Equal(@"""hello \""world""", token.Value);

            Assert.Equal(SyntaxKind.CloseParenthesisToken, lexer.Lex().Kind);
            Assert.Equal(SyntaxKind.EndOfFileToken, lexer.Lex().Kind);
        }

    }
}