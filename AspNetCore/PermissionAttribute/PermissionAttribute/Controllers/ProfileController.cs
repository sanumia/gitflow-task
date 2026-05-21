using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PermissionAttribute.Attributes;
using PermissionAttribute.Models;
using PermissionAttribute.Models.Enums;

namespace PermissionAttribute.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProfileController : ControllerBase
{
    private readonly PermissionDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public ProfileController(PermissionDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [HttpGet("{userId}")]
    [HasPermission(Permissions.GetProfileById)]
    public async Task<IActionResult> GetProfileByUserId(string userId)
    {
        var contact = await _context.Contacts
            .FirstOrDefaultAsync(c => c.OwnerID == userId);

        if (contact == null)
            return NotFound($"No profile found for user ID {userId}");

        return Ok(contact);
    }

    [HttpGet]
    [HasPermission(Permissions.GetProfiles)]
    public async Task<IActionResult> GetAllProfiles()
    {
        var contacts = await _context.Contacts.ToListAsync();
        return Ok(contacts);
    }

    [HttpPost]
    [HasPermission(Permissions.AddProfile)]
    public async Task<IActionResult> AddProfile([FromBody] Contact newContact)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (string.IsNullOrEmpty(newContact.OwnerID))
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Unauthorized();
            newContact.OwnerID = user.Id;
        }

        _context.Contacts.Add(newContact);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetProfileByUserId), new { userId = newContact.OwnerID }, newContact);
    }

    [HttpPut("{id}")]
    [HasPermission(Permissions.UpdateProfile)]
    public async Task<IActionResult> UpdateProfile(int id, [FromBody] Contact updatedContact)
    {
        if (id != updatedContact.ContactId)
            return BadRequest("ID mismatch");

        var existing = await _context.Contacts.FindAsync(id);
        if (existing == null)
            return NotFound();

        existing.Name = updatedContact.Name;
        existing.Address = updatedContact.Address;
        existing.City = updatedContact.City;
        existing.State = updatedContact.State;
        existing.Zip = updatedContact.Zip;
        existing.Email = updatedContact.Email;
        existing.Status = updatedContact.Status;

        _context.Contacts.Update(existing);
        await _context.SaveChangesAsync();

        return Ok(existing);
    }

    [HttpDelete("{id}")]
    [HasPermission(Permissions.DeleteProfile)]
    public async Task<IActionResult> DeleteProfile(int id)
    {
        var contact = await _context.Contacts.FindAsync(id);
        if (contact == null)
            return NotFound();

        _context.Contacts.Remove(contact);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
