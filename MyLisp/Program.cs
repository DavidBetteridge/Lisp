using System;
using System.Linq;

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
                if (parser.DiagnosticBag.Errors.Any())
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    foreach (var error in parser.DiagnosticBag.Errors)
                        Console.WriteLine(error.Message);
                    Console.ResetColor();
                }
                else
                {
                    var statement = parser.ParseBracketedStatement();

                    var binder = new Binder();
                    var boundStatement = binder.Bind(statement);

                    var evalulator = new Evaluator(environment);
                    Console.WriteLine(evalulator.Evaluate(boundStatement));
                }
            }
        }
    }
}
