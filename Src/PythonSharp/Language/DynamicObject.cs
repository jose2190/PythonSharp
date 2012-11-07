﻿namespace PythonSharp.Language
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class DynamicObject : IValues
    {
        private DefinedClass klass;
        private IDictionary<string, object> values = new Dictionary<string, object>();

        public DynamicObject(DefinedClass klass)
        {
            this.klass = klass;
        }

        public DefinedClass Class { get { return this.klass; } }

        public object GetValue(string name)
        {
            if (this.values.ContainsKey(name))
                return this.values[name];

            return this.klass.GetMethod(name);
        }

        public void SetValue(string name, object value)
        {
            this.values[name] = value;
        }

        public bool HasValue(string name)
        {
            if (this.values.ContainsKey(name))
                return true;

            return this.klass.GetMethod(name) != null;
        }

        public object InvokeMethod(string name, BindingEnvironment environment, IList<object> arguments, IDictionary<string, object> namedArguments)
        {
            IFunction method = (IFunction) this.GetValue(name);
            IList<object> args = new List<object>() { this };

            if (arguments != null && arguments.Count > 0)
                foreach (var arg in arguments)
                    args.Add(arg);

            return method.Apply(environment, args, namedArguments);
        }
    }
}
