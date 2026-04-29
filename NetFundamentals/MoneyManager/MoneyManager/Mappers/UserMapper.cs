using MoneyManager.Entities;
using MoneyManager.Models;

namespace MoneyManager.Mappers;

public static class UserMapper
{
    public static UserBaseInfo ToBaseInfo(this User user) => new()
    {
        Id = user.Id,
        Name = user.Name
    };
}
