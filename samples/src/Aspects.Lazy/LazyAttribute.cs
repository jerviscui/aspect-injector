using System;
using AspectInjector.Broker;

namespace Aspects.Lazy
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    [Injection(typeof(LazyAspect))]
    public class LazyAttribute : Attribute
    {

    }
}
