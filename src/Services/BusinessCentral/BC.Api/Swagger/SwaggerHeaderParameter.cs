using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace BC.Api.Swagger
{
    public class SwaggerHeaderParameter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            operation.Parameters =
            [
                new OpenApiParameter
            {
                Name = "X-Source-Header",
                In = ParameterLocation.Header,
                Description = "Custom header for source",
                Required = true
            },
        ];
        }
    }
}
