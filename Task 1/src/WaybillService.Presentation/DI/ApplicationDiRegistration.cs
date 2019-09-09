using Microsoft.Extensions.DependencyInjection;
using WaybillService.Application.Waybills.Paper;
using WaybillService.Application.Waybills.Paper.ContentFilling;
using WaybillService.Application.Waybills.Paper.FileImport;
using WaybillService.Application.Waybills.Paper.StructuralValidation;
using WaybillService.Infrastructure.Waybills;

namespace WaybillService.Presentation.DI
{
    public static class ApplicationDiRegistration
    {
        public static IServiceCollection RegisterApplication(IServiceCollection services)
        {
            services.AddOptions();
            services.RegisterWaybills();
            return services;
        }
        
        private static void RegisterWaybills(this IServiceCollection services)
        {
            services.AddScoped<IWaybillImporter, WaybillImporter>();
            services.AddScoped<IWaybillContentFiller, WaybillContentFiller>();
            services.AddScoped<IWaybillStructuralValidationFiller, WaybillStructuralValidationFiller>();
        }
    }
}
