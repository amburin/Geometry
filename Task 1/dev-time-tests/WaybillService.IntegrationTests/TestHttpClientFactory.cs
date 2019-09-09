using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Hangfire;
using Hangfire.Common;
using Hangfire.States;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WaybillService.Database.Context;
using WaybillService.Presentation;
using Xunit.Abstractions;

namespace WaybillService.IntegrationTests
{
    public static class TestHttpClientFactory
    {
        public static HttpClient Create(
            ITestOutputHelper outputHelper,
            WaybillContext context,
            LogLevel logEventLevel = LogLevel.Debug)
        {
            var server = new TestServer(
                new WebHostBuilder()
                    .UseStartup<Startup>()
                    .ConfigureTestServices(
                        services =>
                        {
                            services.RegisterXUnitLogger(outputHelper, logEventLevel);
                            services
                                .AddScoped<IBackgroundJobClient, ImmediateBackgroundJobClient>();

                            services.AddSingleton(context);
                        }
                    )
                    .UseConfiguration(new ConfigurationFixture().Configuration));
            return server.CreateClient();
        }

        // ReSharper disable once UnusedMember.Local

        private class ImmediateBackgroundJobClient : IBackgroundJobClient
        {
            private readonly IServiceProvider _serviceProvider;

            public ImmediateBackgroundJobClient(IServiceProvider serviceProvider)
            {
                _serviceProvider = serviceProvider;
            }

            public string Create(Job job, IState state)
            {
                var service = _serviceProvider.GetService(job.Type.GetInterfaces().Single());

                ((Task) job.Method.Invoke(service, job.Args.ToArray())).GetAwaiter().GetResult();

                return Guid.NewGuid().ToString();
            }

            public bool ChangeState(string jobId, IState state, string expectedState)
            {
                return true;
            }
        }
    }
}