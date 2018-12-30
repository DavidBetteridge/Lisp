using Xunit;

namespace MyLisp.test
{
    public class MathsTests
    {
        [Theory]
        [InlineData("(+ 1 2 7)", 10)]
        [InlineData("(+ 1 2 (+ 3 4))", 10)]
        [InlineData("(- 2 1 (+ 3 4))", -6)]  //1 - 7
        [InlineData("(* 2 (+ 3 4) 2)", 28)] // 4 * 7
        [InlineData("(/ (* (+ 3 3) 2) 6)", 2)]
        [InlineData("(1+ 12)", 13)]
        [InlineData("(1- 12)", 11)]
        [InlineData("(+)", 0)]
        public void TestMathsOperators(string sourceText, int expectedResult)
        {
            var parser = new Parser(sourceText);
            var statement = parser.ParseBracketedStatement();
            var binder = new Binder();
            var boundStatement = binder.Bind(statement);

            var evalulator = new Evaluator();
            var actualResult = evalulator.Evaluate(boundStatement);

            Assert.Equal(expectedResult, actualResult);
        }
    }
}
