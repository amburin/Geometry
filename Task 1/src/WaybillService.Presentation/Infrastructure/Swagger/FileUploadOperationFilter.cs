using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace WaybillService.Presentation.Infrastructure.Swagger
{
    public class FileUploadOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
            {
                return;
            }

            var formFileParams = context.ApiDescription.ActionDescriptor.Parameters
                .Where(x => x.ParameterType.IsAssignableFrom(typeof(IFormFile)))
                .Select(x => x.Name)
                .ToList();

            if (!formFileParams.Any())
            {
                return;
            }

            var paramsToRemove = new List<IParameter>();

            var filedsToDelete = new[]
            {
                "ContentType",
                "ContentDisposition",
                "Headers",
                "Length",
                "Name",
                "FileName"
            };

            paramsToRemove.AddRange(
                operation.Parameters.Where(x => filedsToDelete.Contains(x.Name, StringComparer.InvariantCultureIgnoreCase)));

            foreach (var param in operation.Parameters)
            {
                paramsToRemove.AddRange(from fileParamName in formFileParams
                                        where param.Name.StartsWith(
                                            fileParamName + ".",
                                            StringComparison.InvariantCultureIgnoreCase)
                    select param);
            }

            paramsToRemove.ForEach(x => operation.Parameters.Remove(x));
            foreach (var paramName in formFileParams)
            {
                var fileParam = new NonBodyParameter
                {
                    Type = "file",
                    Name = paramName,
                    In = "formData"
                };
                operation.Parameters.Add(fileParam);
            }

            operation.Consumes = new List<string> {"multipart/form-data"};
        }
    }
}