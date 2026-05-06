using CustomModelBinders.Models;
using Microsoft.AspNetCore.Mvc;

namespace CustomModelBinders.Controllers;

[ApiController]
[Route("api/person")]
public class PersonController : Controller
{
    [HttpGet("{id}")]
    public IActionResult GetPerson(Person person)
    {
        return Json(person);
    }
}
