using System;
using System.Collections.Generic;
using AspectInjector.Broker;

namespace Aspects.Lazy
{
    [Aspect(Scope.PerInstance)]
    public class LazyAspect
    {
        private readonly Dictionary<string, object> _backFields = new Dictionary<string, object>();

        [Advice(Kind.Around, Targets = Target.Instance | Target.Public | Target.Getter)]
        public object OnGet([Argument(Source.Target)] Func<object[], object> method, [Argument(Source.Name)] string name,
            [Argument(Source.Arguments)] object[] arguments)
        {
            if (!_backFields.TryGetValue(name, out object value))
            {
                var newValue = method(arguments);

                if (_backFields.TryGetValue(name, out object created))
                {
                    value = created;
                }
                else
                {
                    try
                    {
                        _backFields.Add(name, newValue);
                        value = newValue;
                    }
                    catch (ArgumentException)
                    {
                        _backFields.TryGetValue(name, out object created2);
                        value = created2;
                    }
                }
            }

            return value;
        }
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    [Injection(typeof(LazyAspect))]
    public class LazyAttribute : Attribute
    {

    }
}
