using Microsoft.AspNetCore.Mvc;
using ClickUpApp.Domain.Dto.Auth;
using ClickUpApp.Domain.Service;

namespace ClickUpApp.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController] 
	public class AuthenticationController : ControllerBase
	{
		private readonly IAuthenticationService authenticationService;
		public AuthenticationController(IAuthenticationService _authenticationService)
		{
			authenticationService = _authenticationService;
		}

		[HttpPost("Login")]
		public async Task<IActionResult> Login(LoginRequest request)
		{
			return Ok(await authenticationService.LoginAsync(request));
		}

		[HttpPost("Register")]
		public async Task<IActionResult> Register(RegisterRequest request)
		{
			return Ok(await authenticationService.RegisterAsync(request));
		}
	}
}
