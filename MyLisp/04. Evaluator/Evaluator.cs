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

                case BoundNodeKind.DefFunCommand:
                    return EvaluateDefFunCommand((BoundFunctionStatement)boundStatement);

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

                case BoundNodeKind.DivideCommand:
                    return EvaluateDivideCommand((BoundDivideStatement)boundStatement);

                case BoundNodeKind.ModCommand:
                    return EvaluateModCommand((BoundModStatement)boundStatement);

                default:
                    throw new System.Exception("Unknown bound node " + boundStatement.BoundNodeKind);
            }
        }

        private object EvaluateDefFunCommand(BoundFunctionStatement boundStatement)
        {
            var name = (string)boundStatement.FunctionName.Value;
            var parameters = boundStatement.Parameters.Select(parm => (string)parm.Value);
            var body = boundStatement.Body;

            _environment.DefineFunction(name, parameters, body);

            return name;
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

        private object EvaluateModCommand(BoundModStatement boundStatement)
        {
            var lhs = Evaluate(boundStatement.BoundDividendStatement);
            var rhs = Evaluate(boundStatement.BoundDivisorStatement);

            if (lhs is double || rhs is double)
            {
                var dividend = ForceToDouble(lhs);
                var divisor = ForceToDouble(rhs);

                return dividend - divisor * Math.Floor(dividend / divisor);
            }
            else
            {
                return (int)((int)lhs - (int)rhs * Math.Floor((double)(int)lhs / (int)rhs));
            }
        }


        private int EvaluateDividendDivisorCommand(BoundDividendDivisorStatement boundStatement)
        {
            var lhs = (int)Evaluate(boundStatement.BoundDividendStatement);
            var rhs = (int)Evaluate(boundStatement.BoundDivisorStatement);

            return lhs % rhs;
        }

        private object EvaluateOnePlusCommand(BoundOnePlusStatement boundStatement)
        {
            var rhs = Evaluate(boundStatement.BoundStatement);
            if (rhs is int)
                return (int)rhs + 1;
            else
                return (double)rhs + 1.0;
        }

        private object EvaluateOneMinusCommand(BoundOneMinusStatement boundStatement)
        {
            var rhs = Evaluate(boundStatement.BoundStatement);
            if (rhs is int)
                return (int)rhs - 1;
            else
                return (double)rhs - 1.0;
        }

        private object EvaluatePlusCommand(BoundPlusStatement boundStatement)
        {
            var evaluated = boundStatement.BoundStatements.Select(Evaluate);
            var allInts = evaluated.All(v => v is int);

            if (allInts)
                return evaluated.Sum(stat => (int)stat);
            else
                return evaluated.Sum(stat => ForceToDouble(stat));
        }

        private object EvaluateMinusCommand(BoundMinusStatement boundStatement)
        {
            var evaluated = boundStatement.BoundStatements.Select(Evaluate);
            var allInts = evaluated.All(v => v is int);

            if (allInts)
            {
                switch (evaluated.Count())
                {
                    case 0:
                        return 0;
                    case 1:
                        return -(int)evaluated.First();
                    default:
                        var head = (int)evaluated.First();
                        var tail = evaluated.Skip(1);
                        return tail.Aggregate(head, (running, stat) => running - (int)stat);
                }
            }
            else
            {
                switch (evaluated.Count())
                {
                    case 0:
                        return 0;
                    case 1:
                        return -(double)evaluated.First();
                    default:
                        var head = ForceToDouble(evaluated.First());
                        var tail = evaluated.Skip(1);
                        return tail.Aggregate(head, (running, stat) => running - ForceToDouble(stat));
                }
            }
        }

        private object EvaluateMultiplyCommand(BoundMultiplyStatement boundStatement)
        {
            var evaluated = boundStatement.BoundStatements.Select(Evaluate);
            var allInts = evaluated.All(v => v is int);

            if (allInts)
            {
                if (evaluated.Any())
                {
                    var head = (int)evaluated.First();
                    var tail = evaluated.Skip(1);
                    return tail.Aggregate(head, (running, stat) => running * (int)stat);
                }
                else
                {
                    return 1;
                }
            }
            else
            {
                if (evaluated.Any())
                {
                    var head = ForceToDouble(evaluated.First());
                    var tail = evaluated.Skip(1);
                    return tail.Aggregate(head, (running, stat) => running * ForceToDouble(stat));
                }
                else
                {
                    return 1;
                }
            }
        }

        private object EvaluateDivideCommand(BoundDivideStatement boundStatement)
        {
            var evaluated = boundStatement.BoundStatements.Select(Evaluate);
            var allInts = evaluated.All(v => v is int);

            if (allInts)
            {
                var head = (int)evaluated.First();
                if (evaluated.Count() == 1)
                {
                    return 1 / head;
                }
                else
                {
                    var tail = evaluated.Skip(1);
                    return tail.Aggregate(head, (running, stat) => running / (int)stat);
                }
            }
            else
            {
                var head = ForceToDouble(evaluated.First());
                if (evaluated.Count() == 1)
                {
                    return 1 / head;
                }
                else
                {
                    var tail = evaluated.Skip(1);
                    return tail.Aggregate(head, (running, stat) => running / ForceToDouble(stat));
                }
            }
        }
        private double ForceToDouble(object value)
        {
            if (value is int)
                return Convert.ToDouble((int)value);
            else
                return (double)value;
        }
    }
}
