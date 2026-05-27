using Microsoft.AspNetCore.Mvc;
using TimeTracking.Models;
using TimeTracking.Services;

namespace TimeTracking.Controllers;

[ApiController]
[Route("api/profile")]
public class ProfileController(IProfileService service) : ControllerBase
{
    [HttpGet("{id}")]
    public ActionResult<Profile> GetById(int id)
    {
        var profile = service.GetById(id);
        return profile is null
            ? NotFound()
            : Ok(profile);
    }

    [HttpGet]
    public ActionResult<IEnumerable<Profile>> GetAll()
    {
        var profiles = service.GetAll();

        return Ok(profiles);
    }

    [HttpPost]
    public ActionResult<Profile> Add([FromBody] Profile profile)
    {
        if (string.IsNullOrWhiteSpace(profile.Name))
            return BadRequest("Name is required.");

        service.Add(profile);

        return CreatedAtAction(nameof(GetById), new { id = profile.Id }, profile);
    }

    [HttpPut("{id}")]
    public ActionResult Update(int id, [FromBody] Profile updatedProfile)
    {
        if (id != updatedProfile.Id)
            return BadRequest("ID mismatch.");

        bool updated = service.Update(updatedProfile);

        return !updated
            ? NotFound()
            : NoContent();
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        var existing = service.GetById(id);
        bool deleted = service.Delete(id);

        return !deleted
            ? NotFound()
            : NoContent();
    }
}
