﻿using System.Linq;

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

                case BoundNodeKind.MinusCommand:
                    return EvaluateMinusCommand((BoundMinusStatement)boundStatement);

                case BoundNodeKind.MultiplyCommand:
                    return EvaluateMultiplyCommand((BoundMultiplyStatement)boundStatement);

                case BoundNodeKind.DivideCommand:
                    return EvaluateDivideCommand((BoundDivideStatement)boundStatement);

                default:
                    throw new System.Exception("Unknown bound node " + boundStatement.BoundNodeKind);
            }
        }

        private int EvaluatePlusCommand(BoundPlusStatement boundStatement)
        {
            return boundStatement.BoundStatements.Sum(stat => (int)Evaluate(stat));
        }

        private int EvaluateMinusCommand(BoundMinusStatement boundStatement)
        {
            var seed = (int)Evaluate(boundStatement.BoundStatements.First());
            var others = boundStatement.BoundStatements.Skip(1);
            return others.Aggregate(seed, (running, stat) => running - (int)Evaluate(stat));
        }

        private int EvaluateMultiplyCommand(BoundMultiplyStatement boundStatement)
        {
            var seed = (int)Evaluate(boundStatement.BoundStatements.First());
            var others = boundStatement.BoundStatements.Skip(1);
            return others.Aggregate(seed, (running, stat) => running * (int)Evaluate(stat));
        }

        private int EvaluateDivideCommand(BoundDivideStatement boundStatement)
        {
            var seed = (int)Evaluate(boundStatement.BoundStatements.First());
            var others = boundStatement.BoundStatements.Skip(1);
            return others.Aggregate(seed, (running, stat) => running / (int)Evaluate(stat));
        }
    }
}