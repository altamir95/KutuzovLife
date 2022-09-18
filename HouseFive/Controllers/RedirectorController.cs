using HouseFive.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HouseFive.Controllers
{
    [Route("[controller]/[action]")]
    public class RedirectorController : Controller
    {
        private readonly HomeFiveContext _db;
        private readonly ILogger<AppealController> _logger;
        private IWebHostEnvironment _hostingEnvironment;

        public RedirectorController(HomeFiveContext context, ILogger<AppealController> logger, IWebHostEnvironment environment)
        {
            _db = context;
            _logger = logger;
            _hostingEnvironment = environment;
        }

        [HttpGet("{id}")]
        public IActionResult ToAppeal(int id)
        {
            double itemIndex = _db.Appeals.Select(item => item.Id).OrderByDescending(t=>t).ToList().Select((itemId, index) => itemId == id ? index : 0).FirstOrDefault(item => item != 0) + 1;
            int listCount = _db.Appeals.Count();
            int page = (int)Math.Ceiling(itemIndex / 3);
            return Redirect($"/appeal/list/{page}#div{id}");
        }
    }
}
