using Microsoft.Extensions.Options;

namespace Digipolis.Delegation.UnitTests._TestUtilities
{
    public static class TestOptions
    {
        public static IOptions<TOptions> Create<TOptions>(TOptions options) where TOptions : class, new()
        {
            return new TestOptionsWrapper<TOptions>(options);
        }
    }

    public class TestOptionsWrapper<TOptions> : IOptions<TOptions> where TOptions : class, new()
    {
        public TestOptionsWrapper(TOptions options)
        {
            Value = options;
        }

        public TOptions Value { get; }
    }
}
