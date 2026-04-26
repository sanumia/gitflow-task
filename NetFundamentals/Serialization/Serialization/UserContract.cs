using System.Runtime.Serialization;

namespace Serialization;

[DataContract(Name = "user", Namespace = "")]
public class UserContract
{
    [DataMember(Name = "id", Order = 1)]
    public int Id { get; set; }

    [DataMember(Name = "name", Order = 2)]
    public string Name { get; set; } = string.Empty;

    [DataMember(Name = "email", Order = 3)]
    public string Email { get; set; } = string.Empty;

    [DataMember(Name = "created_at", Order = 4)]
    public string CreatedAt { get; set; } = string.Empty;

    [IgnoreDataMember]
    public DateTime DateOfCreation
    {
        get => DateTime.Parse(CreatedAt);
        set => CreatedAt = value.ToString("yyyy-MM-ddTHH:mm:ss");
    }
}
