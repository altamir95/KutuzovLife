using HouseFive.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HouseFive.Controllers
{
    public class ContactsController : Controller
    {

        private readonly HomeFiveContext _db;
        private readonly ILogger<ContactsController> _logger;

        public ContactsController(HomeFiveContext context, ILogger<ContactsController> logger)
        {
            _db = context;
            _logger = logger;
        }
        [Route("контакты")] 
        [HttpGet]
        public IActionResult Index()
        {
            KeyValue keyValue = _db.KeyValues.FirstOrDefault(kv => kv.Key == "contacts");
            if (keyValue == null)
            {
                _db.KeyValues.Add(new KeyValue() { Key = "contacts",Value="Ты можешь заполнить сам просто перейдя в режим разработчика" });
                _db.SaveChanges();
                keyValue = _db.KeyValues.FirstOrDefault(kv => kv.Key == "contacts");
            }
            ViewBag.Contacts = keyValue;
            return View();
        } 
        [HttpPost]
        public IActionResult Change(KeyValue model)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Error");
            }

            KeyValue keyValue = _db.KeyValues.FirstOrDefault(kv => kv.Key == "contacts");

            keyValue.Value = model.Value;

            _db.KeyValues.Update(keyValue);

            _db.SaveChanges();

            return RedirectToAction("Index");
        }


    }
}