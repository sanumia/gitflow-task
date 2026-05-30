using PermissionAttribute.Models;

namespace PermissionAttribute.Services;

public interface IContactService
{
    Task<IEnumerable<Contact>> GetContactsAsync(string userId, string? userRole);
    Task<Contact?> GetContactByIdAsync(int id, string userId, string? userRole);
    Task<Contact> CreateContactAsync(Contact contact, string userId);
    Task<bool> UpdateContactAsync(Contact contact, string userId, string? userRole);
    Task<bool> DeleteContactAsync(int id, string userId, string? userRole);
}
