using HouseFive.Models;
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
    
    public class InitiativeController : Controller
    {
        private readonly HomeFiveContext _db;
        private readonly ILogger<MeetingMinuteController> _logger;

        public InitiativeController(HomeFiveContext context, ILogger<MeetingMinuteController> logger)
        {
            _db = context;
            _logger = logger;
        }
        [Route("инициативы/{page:int=1}")]
        [HttpGet]
        public IActionResult List(int page)
        {
            double ItemCount = _db.Initiatives.Count();
            double howManyTake = 3;

            int howManySkip = (int)((page - 1) * howManyTake);

            double res = ItemCount / howManyTake;

            var pageCount = Math.Ceiling(res);

            ViewBag.PageCount = pageCount;

            if (page > pageCount && page != 1) return RedirectToAction("Error");

            List<Initiative> Initiatives = _db.Initiatives.OrderByDescending(mm => mm.Id).Skip(howManySkip).Take((int)howManyTake).Include(e => e.InitiativeAppeal).ToList().Distinct().ToList();
            ViewBag.Initiatives = Initiatives;
            List<Appeal> appeals = _db.Appeals.ToList();
            ViewBag.Appeals = appeals;

            return View();
        }
        [Route("Инициатива_№{Id}")] 
        public IActionResult Item(int id)
        {
            Initiative initiative = _db.Initiatives.Include(e=>e.InitiativeAppeal).FirstOrDefault(mm => mm.Id == id);

            if (initiative == null)
            {
                initiative = new Initiative();
            }
            ViewBag.Initiative = initiative;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Item(Initiative model)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Error");
            }

            var Initiative = _db.Initiatives.FirstOrDefault(ha => ha.Id == model.Id);

            if (Initiative == null)
            {
                _db.Initiatives.Add(model);
            }
            else
            {
                Initiative.Article = model.Article;

                _db.Initiatives.Update(Initiative);
            }

            await _db.SaveChangesAsync();

            return RedirectToAction("List");
        }
        [HttpPost]
        public IActionResult Delete(Initiative model)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("List");
            }

            var mm = _db.Initiatives.FirstOrDefault(ha => ha.Id == model.Id);

            if (mm != null)
            {
                _db.Initiatives.Remove(mm);
                _db.SaveChanges();
                return RedirectToAction("List");
            }
            return RedirectToAction("Error");

        }

        [HttpPost]
        public IActionResult Change(InitiativeAppeal model)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("List");
            }

            var mm = _db.Initiatives.Include(e => e.InitiativeAppeal).FirstOrDefault(ha => ha.Id == model.InitiativeId); 

            if (mm != null)
            {
                if (mm.InitiativeAppeal != null)
                {
                    _db.InitiativeAppeal.Remove(mm.InitiativeAppeal);
                    _db.SaveChanges();
                }
                _db.InitiativeAppeal.Add(model);
                _db.SaveChanges();
                return RedirectToAction("List");
            }
            return RedirectToAction("Error");
        }



        public async Task<JsonResult> DocUploadImage()
        {
            //

            //
            try
            {
                //ONE ONE ONE
                //if (upload.Length <= 0) return null;


                //var fileName = Guid.NewGuid() + Path.GetExtension(upload.FileName).ToLower();


                //var filePath = Path.Combine(
                //    Directory.GetCurrentDirectory(), "wwwroot/articleimg",
                //    fileName);

                //using (var stream = System.IO.File.Create(filePath))
                //{
                //    upload.CopyTo(stream);
                //}

                //var url = $"{"/articleimg/"}{fileName}";

                //var successMessage = "image is uploaded";

                //dynamic success = Newtonsoft.Json.JsonConvert.DeserializeObject("{ 'uploaded': 1,'fileName': \"'" + fileName + "'\",'url': \"'" + url + "'\", 'error': { 'message': \"'" + successMessage + "'\"}}");
                //return Json(success);
                //ONE ONE ONE

                //var uploads = Path.Combine(_hostingEnvironment.WebRootPath, "images/vehicle-key");
                //Guid.NewGuid() + ".pdf";
                var fileName = $"{Guid.NewGuid()}.jpg";
                var filePath = Path.Combine("wwwroot/articleimg");
                var urls = new List<string>();

                //If folder of new key is not exist, create the folder.
                //if (!Directory.Exists(filePath)) Directory.CreateDirectory(filePath);

                foreach (var contentFile in Request.Form.Files)
                {
                    if (contentFile != null && contentFile.Length > 0)
                    {
                        await contentFile.CopyToAsync(new FileStream($"{filePath}\\{fileName}", FileMode.Create));
                        urls.Add($"/articleimg/{fileName}");
                    }
                }

                return Json(urls);
            }
            catch (Exception e)
            {
                return Json(new { error = new { message = e.Message } });
            }
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
