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
                if (!DisplayErrors(parser.DiagnosticBag))
                {
                    var statement = parser.Parse();
                    if (!DisplayErrors(parser.DiagnosticBag))
                    {
                        var binder = new Binder(sourceText);
                        var boundStatement = binder.Bind(statement);

                        if (!DisplayErrors(binder.DiagnosticBag))
                        {
                            var evalulator = new Evaluator(environment);
                            Console.WriteLine(evalulator.Evaluate(boundStatement));
                        }
                    }
                }
            }
        }

        private static bool DisplayErrors(DiagnosticBag  diagnosticBag)
        {
            if (diagnosticBag.Errors.Any())
            {
                Console.ForegroundColor = ConsoleColor.Red;
                foreach (var error in diagnosticBag.Errors)
                    Console.WriteLine(error.Message);
                Console.ResetColor();

                return true;
            }

            return false;
        }
    }
}
