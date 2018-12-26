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
            var binder = new Binder();
            var boundStatement = binder.Bind(statement);

            var evalulator = new Evaluator();
            Console.WriteLine(evalulator.Evaluate(boundStatement));

            Console.ReadKey(true);
        }
    }
}
