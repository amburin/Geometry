using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.Extensions.Configuration;

namespace WaybillService.IntegrationTests
{
    /// <summary>Provides access to configuration files via XUnit fixture.</summary>
    public class ConfigurationFixture
    {
        public IConfiguration Configuration { get; }
        public IHostingEnvironment HostingEnvironment { get; }

        public ConfigurationFixture()
        {
            Configuration = new ConfigurationBuilder()
                .AddJsonFile($"appsettings.json", true, true)
                .AddEnvironmentVariables()
                .Build();

            HostingEnvironment = new HostingEnvironment {EnvironmentName = "Local"};
        }
    }
}