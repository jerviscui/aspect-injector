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
                lock (_backFields)
                {
                    if (!_backFields.TryGetValue(name, out value))
                    {
                        value = method(arguments);
                        _backFields.Add(name, value);
                    }
                }
            }

            return value;

            //下面会有线程安全问题，有概率相同键产生两个值
            //try
            //{
            //    if (!_backFields.TryGetValue(name, out object value))
            //    {
            //        var newValue = method(arguments);

            //        if (!_backFields.TryGetValue(name, out value))
            //        {
            //            try
            //            {
            //                _backFields.Add(name, newValue);
            //                value = newValue;
            //            }
            //            catch (ArgumentException)
            //            {
            //                _backFields.TryGetValue(name, out value);
            //            }
            //        }
            //    }

            //    return value;
            //}
            //catch (Exception e)
            //{

            //}

            //return "";
        }
    }
}