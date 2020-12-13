using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace Digipolis.Delegation.UnitTests._TestUtilities
{
    public class TestLogger<T> : ILogger<T>
    {
        public List<string> LoggedMessages { get; set; }

        public TestLogger()
        {
            LoggedMessages = new List<string>();
        }

        public TestLogger(List<string> loggedMessages)
        {
            LoggedMessages = loggedMessages;
        }

        public static TestLogger<T> CreateLogger()
        {
            return new TestLogger<T>();
        }

        public static TestLogger<T> CreateLogger(List<string> loggedMessages)
        {
            return new TestLogger<T>(loggedMessages);
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            throw new NotImplementedException();
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            LoggedMessages.Add($"{logLevel}, {state}");
        }
    }
}
