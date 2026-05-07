using Microsoft.AspNetCore.Mvc;
using RateLimit.Attributes;
using RateLimit.Models;
using RateLimit.Services;

namespace RateLimit.Controllers;

[Route("[controller]")]
public class ProfileController : Controller
{
    private readonly IProfileService _profileService;

    public ProfileController(IProfileService profileService)
    {
        _profileService = profileService;
    }

    [RateLimit(maxConcurrentRequests: 3)]
    public async Task<IActionResult> Profiles([FromQuery] ProfileQueryModel query)
    {
        var result = await _profileService.GetProfilesAsync(query);
        return View(result);
    }
}
