using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace MyLisp.test
{
    public class IfTests
    {
        [Fact]
        public void TestTrueBranch()
        {
            var environment = new Environment();

            var actualResult = RunSingleStatement(environment, "(if t 10 (/ 1 0))");

            Assert.Equal(10, actualResult);
        }

        [Fact]
        public void TestFalseBranch()
        {
            var environment = new Environment();

            var actualResult = RunSingleStatement(environment, "(if nil (/ 1 0) 12.3)");

            Assert.Equal(12.3, actualResult);
        }

        private static object RunSingleStatement(Environment environment, string sourceText)
        {
            var parser = new Parser(sourceText);
            var statement = parser.Parse();
            var evaluator = new Evaluator(environment);

            return evaluator.Evaluate(statement);
        }
    }
}
