using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.AspNetCore.Authorization;

namespace ClickUpApp.Nuget.Filter
{
	/// <summary>
	/// Custom swagger header filter
	/// </summary> 
	public class UserTokenFilter : IOperationFilter
	{

		/// <summary>
		/// Add custom header
		/// </summary>
		public void Apply(OpenApiOperation operation, OperationFilterContext context)
		{
			if (context == null || operation == null)
			{
				return;
			}

			if (!context.MethodInfo.DeclaringType.GetCustomAttributes(true).Any(a => a is AuthorizeAttribute))
			{
				return;
			}

			operation.Parameters ??= new List<OpenApiParameter>();

			operation.Parameters.Add(new OpenApiParameter
			{
				Name = "WorkhubContext",
				In = ParameterLocation.Header,
				Description = "Add Custom Session Token",
				Required = true,
				Schema = new OpenApiSchema { Type = "string" },
				Example = new OpenApiString("{\"SessionToken\": \"Token\"}")
			});
		}
	}
}
