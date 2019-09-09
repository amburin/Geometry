using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace WaybillService.IntegrationTests
{
    public static class XUnitLoggerExtensions
    {
        public static IServiceCollection RegisterXUnitLogger(
            this IServiceCollection serviceCollection,
            ITestOutputHelper helper,
            LogLevel logEventLevel = LogLevel.Debug)
        {
            var loggerFactory = CreateLoggerFactory(helper, logEventLevel);
            serviceCollection.AddSingleton(loggerFactory);
            serviceCollection.AddLogging();
            return serviceCollection;
        }

        public static ILoggerFactory CreateLoggerFactory(
            ITestOutputHelper helper,
            LogLevel logEventLevel)
        {
            var loggerFactory = new LoggerFactory();
            loggerFactory.AddProvider(new XUnitLoggerProvider(helper, logEventLevel));
            return loggerFactory;
        }

        private class XUnitLoggerProvider : ILoggerProvider
        {
            private readonly ITestOutputHelper _outputHelper;
            private readonly LogLevel _logLevel;

            public XUnitLoggerProvider(ITestOutputHelper outputHelper, LogLevel logLevel)
            {
                _outputHelper = outputHelper;
                _logLevel = logLevel;
            }

            public ILogger CreateLogger(string categoryName)
            {
                return new XUnitLogger(_outputHelper, _logLevel);
            }

            public void Dispose()
            {
            }
        }

        private class XUnitLogger : ILogger, IDisposable
        {
            private readonly ITestOutputHelper _helper;
            private readonly LogLevel _minimumLevel;

            public XUnitLogger(ITestOutputHelper helper, LogLevel minimumLevel)
            {
                _helper = helper;
                _minimumLevel = minimumLevel;
            }

            public void Log<TState>(
                LogLevel logLevel,
                EventId eventId,
                TState state,
                Exception exception,
                Func<TState, Exception, string> formatter)
            {
                if (logLevel >= _minimumLevel)
                {
                    _helper.WriteLine($"{logLevel:G}:{eventId}:{formatter(state, exception)}");
                }
            }

            public bool IsEnabled(LogLevel logLevel)
            {
                return logLevel >= _minimumLevel;
            }

            public IDisposable BeginScope<TState>(TState state)
            {
                return new XUnitLogger(_helper, _minimumLevel);
            }

            public void Dispose()
            {
            }
        }
    }
}