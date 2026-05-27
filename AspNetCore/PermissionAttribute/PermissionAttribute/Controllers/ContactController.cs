using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PermissionAttribute.Attributes;
using PermissionAttribute.Models;
using PermissionAttribute.Models.Enums;
using PermissionAttribute.Services;
using System.Security.Claims;

namespace PermissionAttribute.Controllers;

[ApiController]
[Route("api/contact")]
public class ContactsController(IContactService contactService, UserManager<IdentityUser> userManager) : ControllerBase
{
    private async Task<(string userId, string? role)> GetCurrentUserInfo()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var user = await userManager.GetUserAsync(User);
        var roles = await userManager.GetRolesAsync(user);

        return (userId, roles.FirstOrDefault());
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var (userId, role) = await GetCurrentUserInfo();
        var contacts = await contactService.GetContactsAsync(userId, role);

        return Ok(contacts);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var (userId, role) = await GetCurrentUserInfo();
        var contact = await contactService.GetContactByIdAsync(id, userId, role);

        return contact is null
            ? NotFound()
            : Ok(contact);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Contact contact)
    {
        var (userId, _) = await GetCurrentUserInfo();
        var created = await contactService.CreateContactAsync(contact, userId);

        return CreatedAtAction(nameof(Get), new { id = created.ContactId }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Contact contact)
    {
        if (id != contact.ContactId) return BadRequest("ID mismatch");
        var (userId, role) = await GetCurrentUserInfo();
        var success = await contactService.UpdateContactAsync(contact, userId, role);

        return !success
            ? NotFound()
            : NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var (userId, role) = await GetCurrentUserInfo();
        var success = await contactService.DeleteContactAsync(id, userId, role);

        return !success
            ? NotFound()
            : NoContent();
    }
}