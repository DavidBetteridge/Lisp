using System;
using System.Linq;

namespace MyLisp
{
    public class Evaluator
    {
        private Environment _environment;

        public Evaluator(Environment environment)
        {
            _environment = environment;
        }

        public object Evaluate(BoundStatement boundStatement)
        {
            switch (boundStatement.BoundNodeKind)
            {
                case BoundNodeKind.Identifier:
                    return EvaluateIdentifier((BoundIdentifierStatement)boundStatement);

                case BoundNodeKind.DefVarCommand:
                    return EvaluateDefVarCommand((BoundDefVarStatement)boundStatement);

                case BoundNodeKind.Literal:
                    return ((BoundLiteralExpression)boundStatement).Value;

                case BoundNodeKind.OnePlusCommand:
                    return EvaluateOnePlusCommand((BoundOnePlusStatement)boundStatement);

                case BoundNodeKind.OneMinusCommand:
                    return EvaluateOneMinusCommand((BoundOneMinusStatement)boundStatement);

                case BoundNodeKind.PlusCommand:
                    return EvaluatePlusCommand((BoundPlusStatement)boundStatement);

                case BoundNodeKind.MinusCommand:
                    return EvaluateMinusCommand((BoundMinusStatement)boundStatement);

                case BoundNodeKind.MultiplyCommand:
                    return EvaluateMultiplyCommand((BoundMultiplyStatement)boundStatement);

                case BoundNodeKind.DividendDivisorCommand:
                    return EvaluateDividendDivisorCommand((BoundDividendDivisorStatement)boundStatement);

                case BoundNodeKind.DivideCommand when (boundStatement.Type == typeof(int)):
                    return EvaluateDivideCommand((BoundDivideStatement)boundStatement);

                case BoundNodeKind.DivideCommand when (boundStatement.Type == typeof(double)):
                    return EvaluateFloatingPointDivideCommand((BoundDivideStatement)boundStatement);

                case BoundNodeKind.ModCommand when (boundStatement.Type == typeof(int)):
                    return EvaluateModCommand((BoundModStatement)boundStatement);

                case BoundNodeKind.ModCommand when (boundStatement.Type == typeof(double)):
                    return EvaluateFloatingPointModCommand((BoundModStatement)boundStatement);

                default:
                    throw new System.Exception("Unknown bound node " + boundStatement.BoundNodeKind);
            }
        }

        private object EvaluateIdentifier(BoundIdentifierStatement boundStatement)
        {
            return _environment.Read(boundStatement.VariableName);
        }

        private object EvaluateDefVarCommand(BoundDefVarStatement boundStatement)
        {
            if (_environment.IsSet(boundStatement.Name))
            {
                return _environment.Read(boundStatement.Name);
            }
            else
            {
                var value = Evaluate(boundStatement.InitialValue);
                _environment.Define(boundStatement.Name, value, boundStatement.Documentation);
                return value;
            }
        }

        private int EvaluateModCommand(BoundModStatement boundStatement)
        {
            var lhs = (int)Evaluate(boundStatement.BoundDividendStatement);
            var rhs = (int)Evaluate(boundStatement.BoundDivisorStatement);

            return (int)(lhs - rhs * Math.Floor((double)lhs / rhs));
        }

        private double EvaluateFloatingPointModCommand(BoundModStatement boundStatement)
        {
            var lhs = Math.Floor(EvaluateAsDouble(boundStatement.BoundDividendStatement));
            var rhs = EvaluateAsDouble(boundStatement.BoundDivisorStatement);

            return lhs - rhs * Math.Floor(lhs / rhs);
        }

        private int EvaluateDividendDivisorCommand(BoundDividendDivisorStatement boundStatement)
        {
            var lhs = (int)Evaluate(boundStatement.BoundDividendStatement);
            var rhs = (int)Evaluate(boundStatement.BoundDivisorStatement);

            return lhs % rhs;
        }

        private int EvaluateOnePlusCommand(BoundOnePlusStatement boundStatement)
        {
            var rhs = (int)Evaluate(boundStatement.BoundStatement);
            return rhs + 1;
        }

        private int EvaluateOneMinusCommand(BoundOneMinusStatement boundStatement)
        {
            var rhs = (int)Evaluate(boundStatement.BoundStatement);
            return rhs - 1;
        }

        private int EvaluatePlusCommand(BoundPlusStatement boundStatement)
        {
            return boundStatement.BoundStatements.Sum(stat => (int)Evaluate(stat));
        }

        private int EvaluateMinusCommand(BoundMinusStatement boundStatement)
        {
            switch (boundStatement.BoundStatements.Count())
            {
                case 0:
                    return 0;

                case 1:
                    return -(int)Evaluate(boundStatement.BoundStatements.First());

                default:
                    var head = (int)Evaluate(boundStatement.BoundStatements.First());
                    var tail = boundStatement.BoundStatements.Skip(1);
                    return tail.Aggregate(head, (running, stat) => running - (int)Evaluate(stat));
            }

        }

        private int EvaluateMultiplyCommand(BoundMultiplyStatement boundStatement)
        {
            if (boundStatement.BoundStatements.Any())
            {
                var head = (int)Evaluate(boundStatement.BoundStatements.First());
                var tail = boundStatement.BoundStatements.Skip(1);
                return tail.Aggregate(head, (running, stat) => running * (int)Evaluate(stat));
            }
            else
            {
                return 1;
            }
        }

        private int EvaluateDivideCommand(BoundDivideStatement boundStatement)
        {
            var head = (int)Evaluate(boundStatement.BoundStatements.First());
            if (boundStatement.BoundStatements.Count() == 1)
            {
                return 1 / head;
            }
            else
            {
                var tail = boundStatement.BoundStatements.Skip(1);
                return tail.Aggregate(head, (running, stat) => running / (int)Evaluate(stat));
            }
        }

        private double EvaluateFloatingPointDivideCommand(BoundDivideStatement boundStatement)
        {
            var firstStatement = boundStatement.BoundStatements.First();
            var head = EvaluateAsDouble(firstStatement);
            if (boundStatement.BoundStatements.Count() == 1)
            {
                return 1 / head;
            }
            else
            {
                var tail = boundStatement.BoundStatements.Skip(1);
                return tail.Aggregate(head, (running, stat) => running / EvaluateAsDouble(stat));
            }
        }

        private double EvaluateAsDouble(BoundStatement statement)
        {
            if (statement.Type == typeof(int))
                return Convert.ToDouble((int)Evaluate(statement));
            else
                return (double)Evaluate(statement);
        }
    }
}
