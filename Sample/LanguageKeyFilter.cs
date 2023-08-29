using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Sample;

public class LanguageKeyFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        operation.Parameters ??= new List<OpenApiParameter>();
        operation.Parameters.Add(new OpenApiParameter()
        {
            Name = "Language",
            In = ParameterLocation.Header,
            Required = true,
            AllowEmptyValue = false,
            Schema = new OpenApiSchema() { Type = "string" },
            Example = new OpenApiString("en")
        });
    }
}