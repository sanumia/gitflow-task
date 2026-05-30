using Microsoft.AspNetCore.Mvc;

namespace CustomJsonFormatter.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProfileController(JsonDbContext context) : ControllerBase
{
    [HttpGet("{id}")]
    public ActionResult GetById(int id)
    {
        var profile = context.Profiles.Find(id);

        return profile is null
            ? NotFound()
            : Ok(profile);
    } 
}
