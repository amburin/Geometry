using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using WaybillService.Application.Files;
using WaybillService.Application.Waybills.Paper;
using WaybillService.Application.Waybills.Paper.ContentFilling;
using WaybillService.Application.Waybills.Provider;
using WaybillService.Core;
using WaybillService.Core.Waybills;
using WaybillService.Core.Waybills.Events;
using WaybillService.Core.Waybills.Paper;
using WaybillService.Database.Context;
using WaybillService.Infrastructure.EventPublisher;
using WaybillService.Infrastructure.Files;
using WaybillService.Infrastructure.Waybills;
using WaybillService.Infrastructure.Waybills.Handlers;

namespace WaybillService.Presentation.DI
{
    public static class InfrastructureDiRegistration
    {
        public static IServiceCollection RegisterInfrastructure(
            IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<WaybillContext>(
                x => x.UseInMemoryDatabase("Waybills").UseLoggerFactory(new NullLoggerFactory()));

            services.AddOptions();

            services.RegisterRepositories();
            services.RegisterHandlers();

            services.AddScoped<ExcelFileFactory>();
            services.AddScoped<IFileRepository, FakeFileRepository>();
            services.AddScoped<IWaybillFileRepository, WaybillFileRepository>();
            services.AddScoped<IWaybillProvider, WaybillProvider>();
            services.AddScoped<IWaybillFileParser, FakeWaybillFileParser>();
            services.AddScoped<IEventPublisher, EventPublisher>();
            services.AddTransient<IExcelFileFactory, ExcelFileFactory>();

            return services;
        }

        private static IServiceCollection RegisterRepositories(this IServiceCollection services)
        {
            services.AddScoped<IWaybillFileRepository, WaybillFileRepository>();
            services.AddScoped<IWaybillRepository, WaybillRepository>();
            services.AddScoped<IPaperWaybillRepository, WaybillRepository>();

            return services;
        }

        private static IServiceCollection RegisterHandlers(this IServiceCollection services)
        {
            services.AddScoped<IHandler<PaperWaybillImported>, ParseWaybill>();
            services.AddScoped<IHandler<WaybillParsed>, ValidateWaybill>();

            return services;
        }
    }
}