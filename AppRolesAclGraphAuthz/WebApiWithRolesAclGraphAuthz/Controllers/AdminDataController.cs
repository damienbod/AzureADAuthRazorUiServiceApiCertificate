﻿using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web;

namespace WebApiWithRolesAclGraphAuthz.Controllers
{
    [Authorize]
    // [Authorize(Policy = "p-web-api-with-roles-admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class AdminDataController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new List<string> { "admin data 1", "admin data 2" });
        }
    }
}
