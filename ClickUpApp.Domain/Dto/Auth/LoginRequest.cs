namespace ClickUpApp.Domain.Dto.Auth
{
    /// <summary>
    /// Login Request
    /// </summary>
    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
