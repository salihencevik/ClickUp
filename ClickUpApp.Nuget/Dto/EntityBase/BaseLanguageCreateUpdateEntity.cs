
namespace ClickUpApp.Nuget
{
    public abstract class BaseLanguageCreateUpdateEntity : BaseIdCreateUpdateEntity
    {
        public int RecordId { get; set; }
        public string Name { get; set; } = null!;
        public byte LanguageId { get; set; }
    }
}
