using System.Linq;

namespace MyLisp
{
    class Evaluator
    {

        internal object Evaluate(BoundStatement boundStatement)
        {
            switch (boundStatement.BoundNodeKind)
            {
                case BoundNodeKind.Literal:
                    return ((BoundLiteralExpression)boundStatement).Value;

                case BoundNodeKind.PlusCommand:
                    return EvaluatePlusCommand((BoundPlusStatement)boundStatement);

                default:
                    throw new System.Exception("Unknown bound node " + boundStatement.BoundNodeKind);
            }
        }

        private int EvaluatePlusCommand(BoundPlusStatement boundStatement)
        {
            return boundStatement.BoundStatements.Sum(stat => (int)Evaluate(stat));
        }
    }
}
