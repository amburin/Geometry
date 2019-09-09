using System.Collections.Generic;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace WaybillService.Presentation.Infrastructure.Swagger
{
    public class CompanyIdHeaderParameterOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
            {
                operation.Parameters = new List<IParameter>();
            }

            operation.Parameters.Add(new NonBodyParameter
            {
                Name = HeaderNames.CompanyIdHeaderName,
                In = "header",
                Description = "Company Id. Use it instead of metazonSupplierId",
                Required = true,
                Type = "string"
            });
        }
    }
}
