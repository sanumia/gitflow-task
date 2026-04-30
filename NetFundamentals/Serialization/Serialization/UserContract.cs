using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Serialization;

[DataContract(Name = "user", Namespace = "")]
public class UserContract
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;

    [JsonPropertyName("created_at")]
    public string CreatedAt { get; set; } = string.Empty;

    [JsonIgnore]
    public DateTime DateOfCreation
    {
        get => DateTime.Parse(CreatedAt);
        set => CreatedAt = value.ToString("yyyy-MM-ddTHH:mm:ss");
    }
}
