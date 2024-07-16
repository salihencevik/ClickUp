using System.Text;
using ClickUpApp.Nuget;
using Newtonsoft.Json;
using ClickUpApp.Nuget.Dto;
using ClickUpApp.Nuget.Helper;
using System.Security.Claims;
using ClickUpApp.Domain.Service;
using ClickUpApp.Domain.Entities;
using ClickUpApp.Domain.Dto.Auth;
using Microsoft.Extensions.Options; 
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.DependencyInjection;
using ClickUpApp.Nuget.CustomException;

namespace ClickUpApp.Business.Service
{ 
	public class AuthenticationService : IAuthenticationService
	{
		private readonly IUserService userService; 
		private readonly TokenOptions tokenOptions;
		public AuthenticationService(
			IUserService _userService, 
			IOptions<TokenOptions> _tokenOptions 
		)
		{
			tokenOptions = _tokenOptions.Value;
			userService = _userService; 
		}

		public async Task<AuthResponseDto> LoginAsync(LoginRequest request)
		{
			Guard.NotNull(request, nameof(LoginRequest));

			if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
			{
				throw new CustomerException("Email and Password fields is required");
			}

			var user = await userService.CheckPasswordAsync(request.Email, request.Password);

			return AuthResponse(user);
		}

		public async Task<AuthResponseDto> RegisterAsync(RegisterRequest request)
		{
			Guard.NotNull<RegisterRequest>(request, nameof(RegisterRequest));

			var user = await userService.FindByEmailAsync(request.Email);

			if (user != null)
			{
				throw new CustomerException("This email is already in use.");
			}

			var newUser = new User
			{
				Name = request.Name,
				Email = request.Email,
				Surname = request.Surname,
				CreateDate = DateTime.Now,
				CountryId = request.CountryId,
				KvkkApprove = request.KvkkApprove, 
				Password = PasswordHelper.HashPassword(request.Password) ?? throw new Exception("Error encountered while creating password")
			};

			var isInsertUser = await userService.InsertUser(newUser);

			if (!isInsertUser)
			{
				throw new Exception("Error encountered while creating user");
			}

			return AuthResponse(newUser);
		}


		private AuthResponseDto AuthResponse(User user)
		{
			var userAuthToken = new UserAuthToken
			{
				Id = user.Id,
				CustomerType = user.CustomerType,
				Email = user.Email,
				Name = user.Name,
				Surname = user.Surname,
				Ticks = DateTime.Now.AddMinutes(tokenOptions.AccessTokenExpiration).Ticks
			};

			return new AuthResponseDto
			{
				Token = WriteToken(user),
				UserToken = Cryptolog.Encrypt(JsonConvert.SerializeObject(userAuthToken)),
				UserAuth = userAuthToken
			};
		}

		private TokenDto WriteToken(User user)
		{
			var accessTokenExpiration = DateTime.Now.AddMinutes(tokenOptions.AccessTokenExpiration);
			var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenOptions.SecurityKey));

			var claims = new List<Claim>
						{
							new Claim(ClaimTypes.Name, $"{user.Name} {user.Surname}"),
							new Claim(ClaimTypes.Email, user.Email),
						};

			JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
				claims: claims,
				notBefore: DateTime.Now,
				issuer: tokenOptions.Issuer,
				audience: tokenOptions.Audience[0],
				expires: accessTokenExpiration,
				signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature));


			var tokenDto = TokenHelper.CreateToken(jwtSecurityToken, accessTokenExpiration,
												   DateTime.Now.AddMinutes(tokenOptions.RefreshTokenExpiration));

			return tokenDto;
		}
	}
}
