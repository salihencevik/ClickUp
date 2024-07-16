using ClickUpApp.Nuget.Middleware;
using Microsoft.AspNetCore.Builder;

namespace ClickUpApp.Nuget.Configure
{
	public static class MiddlewareConfigure
	{
		public static void ConfigureCustomMiddleware(this IApplicationBuilder app)
		{
			app.UseMiddleware<Logware>();
		}
	}
}
