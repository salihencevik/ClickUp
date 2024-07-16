
namespace ClickUpApp.Nuget
{
    public abstract class BaseIdCreateUpdateEntity : BaseIdCreateEntity
    { 
        public DateTime? UpdateDate { get; set; }
    }
}
