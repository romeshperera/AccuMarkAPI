using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AccuMarkers.Controllers
{
    [Produces("application/json")]
    [Route("Health")]
    public class HealthController : Controller
    {
        // GET: Health
        [HttpGet]
        public string Get()
        {
            return "AccuMarkers - Healthy";
        }
    }
}
