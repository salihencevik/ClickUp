using ClickUpApp.Nuget;
using ClickUpApp.Nuget.Dto;

namespace ClickUpApp.Domain.Dto.Auth
{
    public class AuthResponseDto
    {
        public TokenDto Token { get; set; }
        public string UserToken { get; set; }
        public UserAuthToken UserAuth { get; set; }
    }
}
