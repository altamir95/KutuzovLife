using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks; 

namespace HouseFive.Controllers
{
    public class AnalyticsController:Controller
    {
        [Route("аналитика")]
        [HttpGet]
        public IActionResult Index(int page)
        {
            return View();
        }
    }
}
