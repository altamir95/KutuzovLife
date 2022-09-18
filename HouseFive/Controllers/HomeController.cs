using HouseFive.Models;
using HouseFive.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HouseFive.Controllers
{
    public class HomeController : Controller
    {
        private readonly HomeFiveContext _db;
        private readonly ILogger<HomeController> _logger;
        private IWebHostEnvironment _hostingEnvironment;

        public HomeController(HomeFiveContext context, ILogger<HomeController> logger, IWebHostEnvironment environment)
        {
            _db = context;
            _logger = logger;
            _hostingEnvironment = environment;
        }

        public IActionResult Index()
        {
            List<HouseAdviser> houseAdvisers = _db.HouseAdvisers.ToList();
            ViewBag.HouseAdvisers = houseAdvisers;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(HouseAdviserViewModel model)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index");
            }


            var houseAdviser = _db.HouseAdvisers.FirstOrDefault(ha => ha.Id == model.Id);

            if (houseAdviser == null)
            {
                houseAdviser = new HouseAdviser()
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Position = model.Position,
                    Quote = model.Quote
                };
                _db.HouseAdvisers.Add(houseAdviser);
                _db.SaveChanges();
                houseAdviser = _db.HouseAdvisers.FirstOrDefault(ha => ha.Position == houseAdviser.Position && ha.LastName == houseAdviser.LastName && ha.FirstName == houseAdviser.FirstName);
            }
            else
            {
                houseAdviser.LastName = model.LastName;
                houseAdviser.FirstName = model.FirstName;
                houseAdviser.Position = model.Position;
                houseAdviser.Quote = model.Quote;
            }

            if (model.Avatar != null)
            {
                string uploads = Path.Combine(_hostingEnvironment.WebRootPath, "images");

                string filePath = Path.Combine(uploads, $"{houseAdviser.Id}.jpg");
                using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await model.Avatar.CopyToAsync(fileStream);
                }
                houseAdviser.ImgURl = $"images/{houseAdviser.Id}.jpg";
            }
            _db.HouseAdvisers.Update(houseAdviser);

            _db.SaveChanges();

            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> Delete(HouseAdviser model)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index");
            } 

            var houseAdviser = await _db.HouseAdvisers.FirstOrDefaultAsync(ha => ha.Id == model.Id);

            if (houseAdviser != null)
            {
                _db.HouseAdvisers.Remove(houseAdviser);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return NotFound();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
