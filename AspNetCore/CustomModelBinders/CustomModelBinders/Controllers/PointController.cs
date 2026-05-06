using CustomModelBinders.Models;
using Microsoft.AspNetCore.Mvc;

namespace CustomModelBinders.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PointController : Controller
{
    [HttpGet]
    public IActionResult GetPoint(Point point)
    {
        return Json(point);
    }
}
