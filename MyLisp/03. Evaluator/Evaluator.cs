using System;
using System.Linq;

namespace MyLisp
{
    public class Evaluator
    {
        private Environment _environment;

        private enum Constants
        {
            NIL,
            T
        }
        public Evaluator(Environment environment)
        {
            _environment = environment;
        }

        public object Evaluate(Statement boundStatement)
        {
            switch (boundStatement.StatementNodeKind)
            {
                case NodeKind.IfCommand:
                    return EvaluateIfCommand((IfStatement)boundStatement);

                case NodeKind.FunctionCall:
                    return EvaluateFunctionCall((FunctionCallStatement)boundStatement);

                case NodeKind.Identifier:
                    return EvaluateIdentifier((IdentifierStatement)boundStatement);

                case NodeKind.DefVarCommand:
                    return EvaluateDefVarCommand((DefVarStatement)boundStatement);

                case NodeKind.DefFunCommand:
                    return EvaluateDefFunCommand((FunctionStatement)boundStatement);

                case NodeKind.Literal:
                    return ((LiteralExpression)boundStatement).Value;

                case NodeKind.OnePlusCommand:
                    return EvaluateOnePlusCommand((OnePlusStatement)boundStatement);

                case NodeKind.OneMinusCommand:
                    return EvaluateOneMinusCommand((OneMinusStatement)boundStatement);

                case NodeKind.PlusCommand:
                    return EvaluatePlusCommand((PlusStatement)boundStatement);

                case NodeKind.MinusCommand:
                    return EvaluateMinusCommand((MinusStatement)boundStatement);

                case NodeKind.MultiplyCommand:
                    return EvaluateMultiplyCommand((MultiplyStatement)boundStatement);

                case NodeKind.DividendDivisorCommand:
                    return EvaluateDividendDivisorCommand((DividendDivisorStatement)boundStatement);

                case NodeKind.DivideCommand:
                    return EvaluateDivideCommand((DivideStatement)boundStatement);

                case NodeKind.ModCommand:
                    return EvaluateModCommand((ModStatement)boundStatement);

                case NodeKind.Empty:
                    return EvaluateEmptyCommand((EmptyStatement)boundStatement);

                case NodeKind.Integer:
                    return EvaluateInteger((NumericStatement)boundStatement);

                case NodeKind.FloatingPoint:
                    return EvaluateFloatingPoint((FloatingPointStatement)boundStatement);

                default:
                    throw new System.Exception("Unknown bound node " + boundStatement.StatementNodeKind);
            }
        }

        private object EvaluateFloatingPoint(FloatingPointStatement boundStatement)
        {
            return boundStatement.Value;
        }

        private object EvaluateInteger(NumericStatement boundStatement)
        {
            return boundStatement.Value;
        }

        private object EvaluateIfCommand(IfStatement boundStatement)
        {
            var predicate = (bool)Evaluate(boundStatement.Predicate);
            if (predicate)
                return Evaluate(boundStatement.TrueBranch);
            else
                return Evaluate(boundStatement.FalseBranch);
        }

        private object EvaluateEmptyCommand(EmptyStatement boundStatement)
        {
            return Constants.NIL;
        }

        private object EvaluateFunctionCall(FunctionCallStatement boundStatement)
        {
            var currentEnvironment = _environment;
            var functionCall = _environment.GetFunction(boundStatement.FunctionName);
            var arguments = functionCall.Parameters
                                .Zip(boundStatement.Args, (name, value) => (name, Evaluate(value)))
                                .ToArray();

            _environment = new Environment();
            foreach (var (name, value) in arguments)
            {
                _environment.Define(name, value, "");
            }

            var result = Evaluate(functionCall.Body);

            _environment = currentEnvironment;

            return result;
        }

        private object EvaluateDefFunCommand(FunctionStatement boundStatement)
        {
            var name = boundStatement.FunctionName;
            var parameters = boundStatement.Parameters;
            var body = boundStatement.Body;

            _environment.DefineFunction(name, parameters, body);

            return name;
        }

        private object EvaluateIdentifier(IdentifierStatement boundStatement)
        {
            if (boundStatement.VariableName.ToLower() == "t") return true;
            if (boundStatement.VariableName.ToLower() == "nil") return false;

            return _environment.Read(boundStatement.VariableName);
        }

        private object EvaluateDefVarCommand(DefVarStatement boundStatement)
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

        private object EvaluateModCommand(ModStatement boundStatement)
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


        private int EvaluateDividendDivisorCommand(DividendDivisorStatement boundStatement)
        {
            var lhs = (int)Evaluate(boundStatement.BoundDividendStatement);
            var rhs = (int)Evaluate(boundStatement.BoundDivisorStatement);

            return lhs % rhs;
        }

        private object EvaluateOnePlusCommand(OnePlusStatement boundStatement)
        {
            var rhs = Evaluate(boundStatement.BoundStatement);
            if (rhs is int)
                return (int)rhs + 1;
            else
                return (double)rhs + 1.0;
        }

        private object EvaluateOneMinusCommand(OneMinusStatement boundStatement)
        {
            var rhs = Evaluate(boundStatement.BoundStatement);
            if (rhs is int)
                return (int)rhs - 1;
            else
                return (double)rhs - 1.0;
        }

        private object EvaluatePlusCommand(PlusStatement boundStatement)
        {
            var evaluated = boundStatement.BoundStatements.Select(Evaluate);
            var allInts = evaluated.All(v => v is int);

            if (allInts)
                return evaluated.Sum(stat => (int)stat);
            else
                return evaluated.Sum(stat => ForceToDouble(stat));
        }

        private object EvaluateMinusCommand(MinusStatement boundStatement)
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

        private object EvaluateMultiplyCommand(MultiplyStatement boundStatement)
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

        private object EvaluateDivideCommand(DivideStatement boundStatement)
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
            else if (value is double)
                return (double)value;
            else
                return 0;
        }
    }
}
