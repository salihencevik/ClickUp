using System.Text;
using ClickUpApp.Nuget;  
using ClickUpApp.Nuget.Filter;
using Microsoft.OpenApi.Models; 
using ClickUpApp.Nuget.Configure; 
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer; 

internal class Program
{
	private static void Main(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);

		builder.Services.AddControllers();

		builder.Services.AddAuthorization();

		//builder.Services.AddDbContext<ClickUpContext>(p => p.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));

		builder.Services.Configure<TokenOptions>(builder.Configuration.GetSection(nameof(TokenOptions)));

		builder.Services.ConfigureBaseService(builder.Configuration); 

		builder.Services.AddMemoryCache();

		builder.Services.AddAuthentication(options =>
		{
			options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
			options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
		}).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, opts =>
		{
			var tokenOptions = builder.Configuration.GetSection("TokenOptions").Get<TokenOptions>();
			opts.TokenValidationParameters = new TokenValidationParameters()
			{
				ValidIssuer = tokenOptions.Issuer,
				ValidAudience = tokenOptions.Audience[0],
				IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenOptions.SecurityKey)),
				ValidateIssuerSigningKey = true,
				ValidateAudience = true,
				ValidateIssuer = true,
				ValidateLifetime = true,
				ClockSkew = TimeSpan.Zero
			};
		});

		builder.Services.AddEndpointsApiExplorer();
		builder.Services.AddSwaggerGen(
			options =>
			{
				options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
				{
					Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
					Name = "Authorization",
					In = ParameterLocation.Header,
					Type = SecuritySchemeType.Http,
					BearerFormat = "JWT",
					Scheme = "bearer"
				});

				options.AddSecurityRequirement(new OpenApiSecurityRequirement{    {
					new OpenApiSecurityScheme
					{
						Reference = new OpenApiReference
						{
							Type = ReferenceType.SecurityScheme,
							Id = JwtBearerDefaults.AuthenticationScheme
						}
					},
					new string[] {}
						}
					});

				options.OperationFilter<UserTokenFilter>();
			});

		var app = builder.Build();

		// Configure the HTTP request pipeline.
		if (app.Environment.IsDevelopment())
		{
			app.UseSwagger();
			app.UseSwaggerUI();
		}

		app.UseHttpsRedirection();

		app.UseCors(x => x
		   .AllowAnyOrigin()
		   .AllowAnyMethod()
		   .AllowAnyHeader());

		app.UseAuthentication();
		app.UseAuthorization();
		app.ConfigureCustomMiddleware();
		app.MapControllers();
		app.Run();
	}
}