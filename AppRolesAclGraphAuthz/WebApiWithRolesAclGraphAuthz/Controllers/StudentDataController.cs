﻿using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web;

namespace WebApiWithRolesAclGraphAuthz.Controllers
{
    [Authorize(Policy = "p-web-api-with-roles-student")]
    [ApiController]
    [Route("api/[controller]")]
    public class StudentDataController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new List<string> { "student data 1", "student data 2" });
        }
    }
}
