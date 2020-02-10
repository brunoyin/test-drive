using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace LdapAuthDemo.Controllers
{
    [Route("/")]
    [ApiController]
    [Authorize]
    public class HomeController: ControllerBase
    {
        // GET home
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET /healthcheck
        [HttpGet("healthcheck")]
        public ActionResult<string> HealthCheck()
        {
            return string.Format("I am healthy at {0}: {1}", DateTime.Now, User.Identity.Name );
        }
    }
}
