using Xunit;

namespace MyLisp.test
{
    public class FunctionTests
    {
        [Fact]
        public void TestDefFun()
        {
            var environment = new Environment();

            RunSingleStatement(environment, "(deffun add (x y) (+ x y))");
            RunSingleStatement(environment, "(defvar a 10)");
            RunSingleStatement(environment, "(defvar b (* 10 10))");
            var actualResult = RunSingleStatement(environment, "(add a b)");

            Assert.Equal(110, actualResult);
        }
        private static object RunSingleStatement(Environment environment, string sourceText)
        {
            var parser = new Parser(sourceText);
            var statement = parser.ParseBracketedStatement();
            var binder = new Binder();
            var boundStatement = binder.Bind(statement);
            var evaluator = new Evaluator(environment);

            return evaluator.Evaluate(boundStatement);
        }
    }
}
