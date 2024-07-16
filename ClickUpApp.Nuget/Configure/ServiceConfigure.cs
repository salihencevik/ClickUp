using System.Reflection;
using ClickUpApp.Nuget.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ClickUpApp.Nuget.Configure
{
	public static class ServiceConfigure
	{
		public static void ConfigureBaseService(this IServiceCollection services, IConfiguration config)
		{
			services.AddSingleton<ILoggerService, LoggerService>();
			services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
			services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
			services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));
		} 
	}
}
