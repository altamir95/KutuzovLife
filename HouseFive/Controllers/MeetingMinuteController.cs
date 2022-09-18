using HouseFive.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace HouseFive.Controllers
{
    public class MeetingMinuteController : Controller
    {
        private readonly HomeFiveContext _db;
        private readonly ILogger<MeetingMinuteController> _logger;

        public MeetingMinuteController(HomeFiveContext context, ILogger<MeetingMinuteController> logger)
        {
            _db = context;
            _logger = logger;
        }
        [Route("протоколыВстреч/{page:int=1}")]
        [HttpGet]
        public IActionResult List(int page)
        {
            double ItemCount = _db.MeetingMinutes.Count();
            double howManyTake = 3;

            int howManySkip = (int)((page - 1) * howManyTake);


            double res = ItemCount / howManyTake;

            var pageCount= Math.Ceiling(res);

            ViewBag.PageCount = pageCount;

            if (page > pageCount && page != 1) return RedirectToAction("Error");


            List<MeetingMinutes> meetingMinutes = _db.MeetingMinutes.OrderByDescending(mm => mm.Title).Skip(howManySkip).Take((int)howManyTake).ToList();
            ViewBag.MeetingMinutes = meetingMinutes;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> List(MeetingMinutes model)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Error");
            }

            var meetingMinutes = _db.MeetingMinutes.FirstOrDefault(ha => ha.Id == model.Id);
            if (meetingMinutes == null)
            {
                _db.MeetingMinutes.Add(model);
            }
            else
            {
                meetingMinutes.Title = model.Title;
                meetingMinutes.Article = model.Article;
                meetingMinutes.DocURl = model.DocURl;

                _db.MeetingMinutes.Update(meetingMinutes);
            }

            await _db.SaveChangesAsync();

            return RedirectToAction("List");
        }


        [HttpPost]
        public async Task<IActionResult> Delete(MeetingMinutes model)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("List");
            }

            var mm = await _db.MeetingMinutes.FirstOrDefaultAsync(ha => ha.Id == model.Id);

            if (mm != null)
            {
                _db.MeetingMinutes.Remove(mm);
                await _db.SaveChangesAsync();
                return RedirectToAction("List");
            }
            return NotFound();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
