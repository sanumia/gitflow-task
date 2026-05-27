using Microsoft.EntityFrameworkCore;
using PermissionAttribute.Models;
using PermissionAttribute.Models.Enums;

namespace PermissionAttribute.Services;

public class ContactService(PermissionDbContext context) : IContactService
{
    public async Task<IEnumerable<Contact>> GetContactsAsync(string userId, string? userRole)
    {
        var query = context.Contacts.AsQueryable();

        // Business rule: Managers see all, ordinary users see only their own
        if (userRole != Roles.Admin.ToString() && userRole != Roles.Manager.ToString())
        {
            query = query.Where(c => c.OwnerID == userId);
        }

        return await query.ToListAsync();
    }

    public async Task<Contact?> GetContactByIdAsync(int id, string userId, string? userRole)
    {
        var contact = await context.Contacts.FindAsync(id);
        if (contact == null) return null;

        // Permission check: only owner or Admin/Manager can view
        if (contact.OwnerID != userId && userRole != Roles.Admin.ToString() && userRole != Roles.Manager.ToString())
            return null;

        return contact;
    }

    public async Task<Contact> CreateContactAsync(Contact contact, string userId)
    {
        contact.OwnerID = userId;
        contact.Status = ContactStatus.Submitted; // default status
        context.Contacts.Add(contact);
        await context.SaveChangesAsync();
        return contact;
    }

    public async Task<bool> UpdateContactAsync(Contact contact, string userId, string? userRole)
    {
        var existing = await context.Contacts.FindAsync(contact.ContactId);
        if (existing == null) return false;

        // Permission check: only owner, Admin, or Manager can update
        if (existing.OwnerID != userId && userRole != Roles.Admin.ToString() && userRole != Roles.Manager.ToString())
            return false;

        existing.Name = contact.Name;
        existing.Address = contact.Address;
        existing.City = contact.City;
        existing.State = contact.State;
        existing.Zip = contact.Zip;
        existing.Email = contact.Email;
        existing.Status = contact.Status;

        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteContactAsync(int id, string userId, string? userRole)
    {
        var contact = await context.Contacts.FindAsync(id);
        if (contact == null) return false;

        // Only Admin or the owner can delete
        if (contact.OwnerID != userId && userRole != Roles.Admin.ToString())
            return false;

        context.Contacts.Remove(contact);
        await context.SaveChangesAsync();
        return true;
    }
}
