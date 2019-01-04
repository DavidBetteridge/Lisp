using System;

namespace MyLisp
{
    class Program
    {
        static void Main(string[] args)
        {
            var environment = new Environment();
            while (true)
            {
                Console.Write("? ");
                var sourceText = Console.ReadLine();
                if (sourceText == "") return;

                var parser = new Parser(sourceText);
                var statement = parser.ParseBracketedStatement();
                var binder = new Binder();
                var boundStatement = binder.Bind(statement);

                var evalulator = new Evaluator(environment);
                Console.WriteLine(evalulator.Evaluate(boundStatement));
            }
        }
    }
}
