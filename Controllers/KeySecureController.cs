using KANBAN_COKHI.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace NETCORE3.Controllers
{
  [ApiKey]
  [EnableCors("CorsApi")]
  [AllowAnonymous]
  [Route("api/[controller]")]
  [ApiController]
  public class KeySecureController : ControllerBase
  {
    public KeySecureController()
    {
    }
    [HttpGet]
    public ActionResult Get()
    {
      return Ok("abc");
    }

  }
}