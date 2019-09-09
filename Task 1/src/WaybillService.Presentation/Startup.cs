using System.Linq;
using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WaybillService.Core;
using WaybillService.Infrastructure;
using WaybillService.Infrastructure.EventPublisher;
using WaybillService.Presentation.Authorization;
using WaybillService.Presentation.Infrastructure;
using ApplicationDiRegistration = WaybillService.Presentation.DI.ApplicationDiRegistration;
using InfrastructureDiRegistration = WaybillService.Presentation.DI.InfrastructureDiRegistration;

namespace WaybillService.Presentation
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly IHostingEnvironment _env;

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(x =>
            {
                x.Filters.Add<ExceptionProcessingFilterAttribute>();
            });

            services.AddHangfire(globalConfiguration =>
            {
                globalConfiguration.UseIgnoredAssemblyVersionTypeResolver();
                globalConfiguration.UseMemoryStorage();
            });
            // Workaround for Hangfire to prevent lots of warnings in log ('The required antiforgery request token was not provided ...')
            services.Remove(services.First(x => x.ServiceType == typeof(IAntiforgery)));

            services.AddTransient<ITimeProvider, TimeProvider>();
            
            //features
            SetupDi(services, _configuration);
            
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseMvc();

            app.UseHangfireServer();
            app.UseHangfireDashboard(options: new DashboardOptions
            {
                Authorization = new[] {new HangfireIntranetAuthorizationFilter()}
            });
        }

        // function to use in app and integration tests
        // all di registrations should be placed here
        public static void SetupDi(IServiceCollection serviceCollection, IConfiguration configuration)
        {
            ApplicationDiRegistration.RegisterApplication(serviceCollection);
            InfrastructureDiRegistration.RegisterInfrastructure(serviceCollection, configuration);
        }
    }
}