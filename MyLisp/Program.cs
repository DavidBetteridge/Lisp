using System;

namespace MyLisp
{
    class Program
    {
        static void Main(string[] args)
        {
            var sourceText = "(+ 1 2)";
            var parser = new Parser(sourceText);
            var statement = parser.ParseLine();
            Console.WriteLine(statement.Command);
            Console.ReadKey(true);
        }
    }
}
