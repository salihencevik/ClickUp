using System.Net;
using Newtonsoft.Json;
using ClickUpApp.Nuget.Dto;
using ClickUpApp.Nuget.Helper;
using ClickUpApp.Nuget.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;

namespace ClickUpApp.Nuget.Middleware
{
	public class Logware
	{
		private readonly RequestDelegate _next;
		private readonly ILoggerService _logger;
		public Logware(RequestDelegate next, ILoggerService logger)
		{
			_next = next;
			_logger = logger;
		}

		public async Task InvokeAsync(HttpContext httpContext, IConfiguration configuration)
		{
			Guard.NotNull(httpContext, nameof(HttpContext));
			var endpoint = httpContext.GetEndpoint();
			var canByPassContextGeneration = CanByPassContextGeneration(endpoint);

			try
			{
				if (!canByPassContextGeneration)
				{
					PrepareContext(httpContext, configuration?["ENVIRONMENT"]);
				}

				await _next(httpContext);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex);
				httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
				await httpContext.Response.WriteAsync(ex.Message);
			}
		}

		private void PrepareContext(HttpContext context, string environment)
		{
			if (context.Request.Method == "OPTIONS") // preflight requests doesn't have context...
			{
				return;
			}

			var hasContext = context.Request.Headers.ContainsKey("ClickUpAppContext");

			if (!hasContext)
			{
				if (context.Request.Headers.TryGetValue("Referer", out var referer) && referer.Any(x => x.Contains("/swagger")))
				{
					return;
				}

				throw new Exception("ContextIsEmpty");
			}

			try
			{
				var deserializedContext = context.Request.Headers["ClickUpAppContext"];

				ClickUpUserContext.Current = JsonConvert.DeserializeObject<ClickUpUserContext>(deserializedContext);

				var currenctLanguageId = context.Request.Headers["CurrentLangId"];

				FlattenContext(environment, currenctLanguageId);
			}
			catch (System.Exception ex)
			{
				if (ex is Exception)
				{
					throw;
				}
			}
		}

		private void FlattenContext(string environment, string languageId)
		{
			var tokenDecrypted = Cryptolog.Decrypt(ClickUpUserContext.Current.SessionToken);
			ClickUpUserContext.Current.AuthToken = JsonConvert.DeserializeObject<UserAuthToken>(tokenDecrypted);
			ClickUpUserContext.Current.AuthToken.LanguageId = languageId == "2" ? LanguageEnum.Turkish : LanguageEnum.English;

			ClickUpUserContext.Current.AuthToken.ValidateToken(environment); // token içeriğini valide ediyor
		}
		private bool CanByPassContextGeneration(Endpoint endpoint)
		{
			if (endpoint == null)
			{
				return true;
			}

			var isMethodWhitelisted = endpoint.Metadata.GetMetadata<AuthorizeAttribute>();

			return isMethodWhitelisted == null;
		}
	}
}
