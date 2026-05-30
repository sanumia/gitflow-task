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
public class ContactController : ControllerBase
{
    private readonly IContactService _contactService;
    private readonly ICurrentUserService _currentUserService;

    public ContactController(IContactService contactService, ICurrentUserService currentUserService)
    {
        _contactService = contactService;
        _currentUserService = currentUserService;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var (userId, role) = await _currentUserService.GetCurrentUserInfoAsync();
        var contacts = await _contactService.GetContactsAsync(userId, role);
        return Ok(contacts);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var (userId, role) = await _currentUserService.GetCurrentUserInfoAsync();
        var contact = await _contactService.GetContactByIdAsync(id, userId, role);
        return contact is null ? NotFound() : Ok(contact);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Contact contact)
    {
        var (userId, _) = await _currentUserService.GetCurrentUserInfoAsync();
        var created = await _contactService.CreateContactAsync(contact, userId);
        return CreatedAtAction(nameof(Get), new { id = created.ContactId }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Contact contact)
    {
        if (id != contact.ContactId) return BadRequest("ID mismatch");
        var (userId, role) = await _currentUserService.GetCurrentUserInfoAsync();
        var success = await _contactService.UpdateContactAsync(contact, userId, role);
        return !success ? NotFound() : NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var (userId, role) = await _currentUserService.GetCurrentUserInfoAsync();
        var success = await _contactService.DeleteContactAsync(id, userId, role);
        return !success ? NotFound() : NoContent();
    }
}