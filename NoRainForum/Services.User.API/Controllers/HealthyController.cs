using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.APICommon;

namespace Services.User.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthyController : ControllerBase
    {
        [HttpGet]
        [ApiAuthor]
        public IActionResult Get()
        {
            return Ok();
        }
    }
}