using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using PermissionAttribute.Models;
using PermissionAttribute.Models.Enums;
using System.Security.Claims;

namespace PermissionAttribute.Services;

public class ContactService(PermissionDbContext context, IMapper mapper) : IContactService
{
    public async Task<IEnumerable<Contact>> GetContactsAsync(string userId, string? userRole)
    {
        var query = context.Contacts.AsQueryable();

        bool isAdminOrManager = userRole == Roles.Admin.ToString() || userRole == Roles.Manager.ToString();

        if (!isAdminOrManager)
        {
            query = query.Where(c => c.OwnerID == userId);
        }

        return await query.ToListAsync();
    }

    public async Task<Contact?> GetContactByIdAsync(int id, string userId, string? userRole)
    {
        var contact = await context.Contacts.FindAsync(id);
        if (contact == null) return null;

        bool isOwner = contact.OwnerID == userId;
        bool isAdminOrManager = userRole == Roles.Admin.ToString() || userRole == Roles.Manager.ToString();
        bool canView = isOwner || isAdminOrManager;

        if (!canView)
            return null;

        return contact;
    }

    public async Task<Contact> CreateContactAsync(Contact contact, string userId)
    {
        contact.OwnerID = userId;
        contact.Status = ContactStatus.Submitted;
        context.Contacts.Add(contact);
        await context.SaveChangesAsync();
        return contact;
    }

    public async Task<bool> UpdateContactAsync(Contact contact, string userId, string? userRole)
    {
        var existing = await context.Contacts.FindAsync(contact.ContactId);
        if (existing == null) return false;

        bool isOwner = existing.OwnerID == userId;
        bool isAdminOrManager = userRole == Roles.Admin.ToString() || userRole == Roles.Manager.ToString();
        bool canUpdate = isOwner || isAdminOrManager;

        if (!canUpdate) return false;

        mapper.Map(contact, existing);

        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteContactAsync(int id, string userId, string? userRole)
    {
        var contact = await context.Contacts.FindAsync(id);
        if (contact == null) return false;

        bool isOwner = contact.OwnerID == userId;
        bool isAdmin = userRole == Roles.Admin.ToString();
        bool canDelete = isOwner || isAdmin;

        if (!canDelete)
            return false;

        context.Contacts.Remove(contact);
        await context.SaveChangesAsync();
        return true;
    }
}