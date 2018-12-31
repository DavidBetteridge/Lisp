using System;
using System.Collections.Generic;

namespace MyLisp
{
    public class Environment
    {
        private readonly Dictionary<string, Variable> _variables = new Dictionary<string, Variable>();

        public void Define(string name, object value, string documentation)
        {
            var variable = new Variable()
            {
                Name = name,
                Value = value,
                Documentation = documentation
            };

            _variables[name] = variable;
        }

        internal bool IsSet(string name)
        {
            if (_variables.TryGetValue(name, out var value))
                return value.Value != null;
            else
                return false;
        }
        struct Variable
        {
            public string Name { get; set; }
            public object Value { get; set; }
            public string Documentation { get; set; }
        }

        internal object Read(string name)
        {
            _variables.TryGetValue(name, out var value);
            return value.Value;
        }

        internal bool IsDefined(string name)
        {
            return _variables.TryGetValue(name, out var value); ;
        }

        internal Type ReadType(string name)
        {
            _variables.TryGetValue(name, out var value);
            return value.Value?.GetType();
        }
    }
}
