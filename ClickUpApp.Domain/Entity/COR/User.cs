
using System.ComponentModel.DataAnnotations.Schema;
using ClickUpApp.Nuget;

namespace ClickUpApp.Domain.Entities;

[Table("Users", Schema = "COR")]
public partial class User : BaseIdCreateUpdateEntity
{
    public int CustomerType { get; set; }
    public string Name { get; set; } = null!;
    public string Surname { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public int CountryId { get; set; }
    public bool KvkkApprove { get; set; }
}
