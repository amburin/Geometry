using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace WaybillService.Presentation.Infrastructure.Swagger
{
    public class MultipleFilesUploadOperationFilter : IOperationFilter
    {
        private const int MaxUploadedFiles = 3;

        public void Apply(Operation operation, OperationFilterContext context)
        {
            if (context.ApiDescription.ParameterDescriptions.Any(
                paramDesc =>
                    paramDesc.Type == typeof(IFormFileCollection)))
            {
                var fileCollectionParameters = GetFileCollectionParameters(operation);
                foreach (var fileCollectionParameter in fileCollectionParameters)
                {
                    var singleFileParameters =
                        GenerateMultipleSingleFileParametersFromCollectionOne(
                            fileCollectionParameter);
                    var firstFileParameter = singleFileParameters.FirstOrDefault();
                    if (firstFileParameter != null) // Others are optional
                    {
                        firstFileParameter.Required = true;
                    }

                    operation.Parameters.Remove(fileCollectionParameter);
                    singleFileParameters.ForEach(par => operation.Parameters.Add(par));
                }
            }

            operation.Consumes.Add("multipart/form-data");
        }

        private static List<NonBodyParameter> GetFileCollectionParameters(Operation operation)
        {
            return operation.Parameters.OfType<NonBodyParameter>()
                .Where(par => par.CollectionFormat == "multi" && par.Type == "array")
                .ToList();
        }

        private List<NonBodyParameter> GenerateMultipleSingleFileParametersFromCollectionOne(
            NonBodyParameter fileCollectionParameter)
        {
            return Enumerable.Range(1, MaxUploadedFiles)
                .Select(i => CreateFileParameter($"{fileCollectionParameter.Name}[{i}]"))
                .ToList();
        }

        private NonBodyParameter CreateFileParameter(string parameterName)
        {
            return new NonBodyParameter
            {
                Name = parameterName,
                Type = "file",
                In = "formData"
            };
        }
    }
}