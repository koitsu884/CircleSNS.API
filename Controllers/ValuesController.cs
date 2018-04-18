using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CircleSNS.API.Controllers
{

    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        // GET api/values
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public string Protected()
        {
            return "Protected area";
        }

        [HttpGet("test")]
        [Authorize(Roles = "Administrator")]
        public ActionResult Test()
        {
            return Content(HttpContext.User.Identity.Name);
        }

        [Authorize]
        [HttpGet("claims")]
        public object Claims()
        {
            return User.Claims.Select(c =>
            new
            {
                Type = c.Type,
                Value = c.Value
            });
        }
    }
}
