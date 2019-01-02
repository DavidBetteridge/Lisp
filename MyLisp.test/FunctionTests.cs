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
            var actualResult = RunSingleStatement(environment, "(add 3 2)");

            Assert.Equal(5, actualResult);
        }
        private static object RunSingleStatement(Environment environment, string sourceText)
        {
            var parser = new Parser(sourceText);
            var statement = parser.ParseBracketedStatement();
            var binder = new Binder(environment);
            var boundStatement = binder.Bind(statement);
            var evaluator = new Evaluator(environment);

            return evaluator.Evaluate(boundStatement);
        }
    }
}
