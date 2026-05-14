using Microsoft.AspNetCore.Mvc;
using RateLimit.Attributes;
using RateLimit.Models;
using RateLimit.Services;

namespace RateLimit.Controllers;

[Route("[controller]")]
public class ProfileController(IProfileService profileService) : Controller
{
    [RateLimit(maxConcurrentRequests: 3)]
    public async Task<IActionResult> Profiles([FromQuery] ProfileQueryModel query)
    {
        var result = await profileService.GetProfilesAsync(query);

        return View(result);
    }
}
