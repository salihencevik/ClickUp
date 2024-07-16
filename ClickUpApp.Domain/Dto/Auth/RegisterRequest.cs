namespace ClickUpApp.Domain.Dto.Auth
{
	public class RegisterRequest
	{ 
		public string Name { get; set; }
		public string Surname { get; set; }
		public string Email { get; set; }
		public string Password { get; set; }
		public int CountryId { get; set; }
		public bool KvkkApprove { get; set; }
	}
}
