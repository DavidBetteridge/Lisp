using System;

namespace MyLisp
{
    class Program
    {
        static void Main(string[] args)
        {
            var sourceText = "(+ 1 2)";
            var lexer = new Lexer(sourceText);
            SyntaxToken token;
            do
            {
                token = lexer.Lex();

                if (token.Kind != SyntaxKind.WhitespaceToken &&
                    token.Kind != SyntaxKind.BadToken)
                {
                    Console.WriteLine(token.Kind);
                    //tokens.Add(token);
                }
            } while (token.Kind != SyntaxKind.EndOfFileToken);

            Console.ReadKey(true);
        }
    }
}
