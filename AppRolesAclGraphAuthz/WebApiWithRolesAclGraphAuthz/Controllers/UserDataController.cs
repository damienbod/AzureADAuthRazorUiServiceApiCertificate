using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web;

namespace WebApiWithRolesAclGraphAuthz.Controllers
{
    [Authorize(Policy = "p-web-api-with-roles-user")]
    [ApiController]
    [Route("api/[controller]")]
    public class UserDataController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new List<string> { "user data 1", "user data 2" });
        }
    }
}
