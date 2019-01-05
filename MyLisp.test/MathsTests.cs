using System;
using Xunit;

namespace MyLisp.test
{
    public class MathsTests
    {
        [Theory]
        [InlineData("(+ 1 2 (+ 3 4))", 10)]
        [InlineData("(- 2 1 (+ 3 4))", -6)]  //1 - 7
        [InlineData("(* 2 (+ 3 4) 2)", 28)] // 4 * 7
        [InlineData("(/ (* (+ 3 3) 2) 6)", 2)]
        [InlineData("(1+ 12)", 13)]
        [InlineData("(1- 12)", 11)]
        public void TestCombinedMathsOperators(string sourceText, int expectedResult)
        {
            var actualResult = Run(sourceText);

            Assert.Equal(expectedResult, actualResult);
        }

        [Theory]
        [InlineData("(+)", 0)]
        [InlineData("(+ 1)", 1)]
        [InlineData("(+ 1 2 3 4)", 10)]
        public void TestPlusOperator(string sourceText, int expectedResult)
        {
            var actualResult = Run(sourceText);

            Assert.Equal(expectedResult, actualResult);
        }

        [Theory]
        [InlineData("(+ 1.1 2.2)", 3.3)]
        public void TestFloatingPointPlusOperator(string sourceText, double expectedResult)
        {
            var actualResult = (double)Run(sourceText);
            Assert.Equal(expectedResult, Math.Round(actualResult, 15));
        }

        [Theory]
        [InlineData("(- 10 1 2 3 4)", 0)]
        [InlineData("(- 10)", -10)]
        [InlineData("(-)", 0)]
        [InlineData("(- 1.1 0.1)", 1.0)]
        public void TestMinusOperator(string sourceText, object expectedResult)
        {
            var actualResult = Run(sourceText);

            Assert.Equal(expectedResult, actualResult);
        }

        [Theory]
        [InlineData("(*)", 1)]
        [InlineData("(* 1)", 1)]
        [InlineData("(* 1 2 3 4)", 24)]
        [InlineData("(* 2 1.1)", 2.2)]
        public void TestMultiplyOperator(string sourceText, object expectedResult)
        {
            var actualResult = Run(sourceText);

            Assert.Equal(expectedResult, actualResult);
        }

        [Theory]
        [InlineData("(/ 5.0 2)", 2.5)]
        [InlineData("(/ 5.0 2.0)", 2.5)]
        [InlineData("(/ 4.0)", 0.25)]
        public void TestDoubleDivideOperator(string sourceText, double expectedResult)
        {
            var actualResult = Run(sourceText);

            Assert.Equal(expectedResult, actualResult);
        }


        [Theory]
        [InlineData("(/ 6 2)", 3)]
        [InlineData("(/ 5 2)", 2)]
        [InlineData("(/ 4)", 0)]
        [InlineData("(/ 25 3 2)", 4)]
        [InlineData("(/ -17 6)", -2)]
        public void TestIntegerDivideOperator(string sourceText, int expectedResult)
        {
            var actualResult = Run(sourceText);

            Assert.Equal(expectedResult, actualResult);
        }

        [Theory]
        [InlineData("(% 9 4)", 1)]
        [InlineData("(% -9 4)", -1)]
        [InlineData("(% 9 -4)", 1)]
        [InlineData("(% -9 -4)", -1)]
        public void TestDividendDivisorOperator(string sourceText, int expectedResult)
        {
            var actualResult = Run(sourceText);

            Assert.Equal(expectedResult, actualResult);
        }


        [Theory]
        [InlineData("(mod 5.5 2.5)", .5)]
        public void TestFloatingPointModOperator(string sourceText, double expectedResult)
        {
            var actualResult = Run(sourceText);

            Assert.Equal(expectedResult, actualResult);
        }

        [Theory]
        [InlineData("(mod 9 4)", 1)]
        [InlineData("(mod -9 4)", 3)]
        [InlineData("(mod 9 -4)", -3)]
        [InlineData("(mod -9 -4)", -1)]
        public void TestIntegerModOperator(string sourceText, int expectedResult)
        {
            var actualResult = Run(sourceText);

            Assert.Equal(expectedResult, actualResult);
        }

        [Theory]
        // [InlineData(@"(defvar x)", 0)]
        [InlineData(@"(defvar x 456)", 456)]
        // [InlineData(@"(defvar x 123 ""This is a comment"")", 123)]
        public void TestDefVarOperator(string sourceText, int expectedResult)
        {
            var actualResult = Run(sourceText);

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void TestDefVar()
        {
            var environment = new Environment();

            RunSingleStatement(environment, "(defvar x 10)");
            var actualResult = RunSingleStatement(environment, "(+ x 2)");

            Assert.Equal(12, actualResult);
        }

        private static object Run(string sourceText)
        {
            var environment = new Environment();
            var parser = new Parser(sourceText);
            var statement = parser.Parse();
            var binder = new Binder(sourceText);
            var boundStatement = binder.Bind(statement);


            var evalulator = new Evaluator(environment);
            var actualResult = evalulator.Evaluate(boundStatement);
            return actualResult;
        }

        private static object RunSingleStatement(Environment environment, string sourceText)
        {
            var parser = new Parser(sourceText);
            var statement = parser.Parse();
            var binder = new Binder(sourceText);
            var boundStatement = binder.Bind(statement);
            var evaluator = new Evaluator(environment);

            return evaluator.Evaluate(boundStatement);
        }
    }
}
