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

        struct Variable
        {
            public string Name { get; set; }
            public object Value { get; set; }
            public string Documentation { get; set; }
        }
    }
}
