using Newtonsoft.Json;

namespace ClickUpApp.Nuget.Dto;

public partial class ClickUpUserContext
{
    private static readonly AsyncLocal<ClickUpUserContext> current = new();
    public static ClickUpUserContext Current
    {
        get
        {
            return current.Value ??= new ClickUpUserContext();
        }
        set
        {
            current.Value = value;
        }
    }

    /// <summary>
    /// Auth token
    /// </summary>
    /// <value></value>
    [JsonProperty("SessionToken")]
    public string SessionToken { get; set; }

    /// <summary>
    /// AuthTokenExtended
    /// </summary>
    /// <value></value>
    [System.Text.Json.Serialization.JsonIgnore]
    public UserAuthToken AuthToken { get; set; }
}
