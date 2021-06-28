This package provides Lazy property.
Put `LazyAttribute` attribure on arrow-properties(only getter property).

```csharp
class TestClass
{
    [Lazy]
    public ServiceA ServiceA => new ServiceA(DateTime.Now);
}
```

