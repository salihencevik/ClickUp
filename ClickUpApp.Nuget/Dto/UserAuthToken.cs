namespace ClickUpApp.Nuget.Dto
{
    public class UserAuthToken : BaseIdEntity
    {
        public int CustomerType { get; set; }
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public string Email { get; set; } = null!;
        public long Ticks { get; set; }
        public bool IsProfileCreated { get; set; }
        public LanguageEnum LanguageId { get; set; }
    }
}
