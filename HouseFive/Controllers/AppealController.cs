using HouseFive.Models;
using HouseFive.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace HouseFive.Controllers
{
    public class AppealController : Controller
    {
        private readonly HomeFiveContext _db;
        private readonly ILogger<AppealController> _logger;
        private IWebHostEnvironment _hostingEnvironment;

        public AppealController(HomeFiveContext context, ILogger<AppealController> logger, IWebHostEnvironment environment)
        {
            _db = context;
            _logger = logger;
            _hostingEnvironment = environment;
        }
        [Route("[controller]/[action]/{page:int=1}")]
        [Route("реестрОбращенийЖителей/{page:int=1}")]
        [HttpGet]
        public IActionResult List(int page)
        {
            double ItemCount = _db.Appeals.Count();
            double howManyTake = 3;

            int howManySkip = (int)((page - 1) * howManyTake);

            double res = ItemCount / howManyTake;

            var pageCount = Math.Ceiling(res);

            ViewBag.PageCount = pageCount;

            if (page > pageCount && page != 1) return RedirectToAction("Error");

            List<Appeal> appeals = _db.Appeals.OrderByDescending(mm => mm.Id).Skip(howManySkip).Take((int)howManyTake).Include(a => a.InitiativeAppeal).ToList().Distinct().ToList();
            ViewBag.Appeals = appeals;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Change(Appeal model)
        {

            _db.Appeals.Add(model);

            _db.SaveChanges();

            if (!_hostingEnvironment.IsDevelopment())
            {
                var newAppeal = _db.Appeals.FirstOrDefault(t => t.CreationDate == model.CreationDate && t.Title == model.Title);

                string tEmail = $"<b>Почта:</b> {(String.IsNullOrEmpty(newAppeal.Email) ? "<s>Не указан</s>" : newAppeal.Email)}";
                string tAuthor = $"<b>Автор:</b> {(String.IsNullOrEmpty(newAppeal.Author) ? "<s>Не указан</s>" : newAppeal.Author)}";
                string tPhoneNum = $"<b>Номер телефона:</b> {(String.IsNullOrEmpty(newAppeal.PhoneNumber) ? "<s>Не указан</s>" : newAppeal.PhoneNumber)}";
                string tLink = $"<a href='https://кутузовлайф.рф/Redirector/ToAppeal/{newAppeal.Id}'>ПЕРЕЙТИ К ОБРАЩЕНИЮ ЖИТЕЛЯ</a>";

                string tApiId = "1997178967:AAGAGltXFk7vVTGP0UXTPXbR4sjHBeMUy6I";
                string tChatId = "-1001569807970";

                var telegramMessage = $"<b>НОВОЕ ОБРАЩЕНИЕ</b>\n\n{tAuthor}\n\n{tPhoneNum}\n\n{tEmail}\n\n{tLink}\n\n";

                var botClient = new TelegramBotClient(tApiId);

                var t = await botClient.SendTextMessageAsync(tChatId, telegramMessage, ParseMode.Html, disableWebPagePreview: true);
            }


            return RedirectToAction("List");
        }
        [HttpPost]
        public IActionResult Delete(Appeal model)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("List");
            }

            var mm = _db.Appeals.FirstOrDefault(ha => ha.Id == model.Id);

            if (mm != null)
            {
                _db.Appeals.Remove(mm);
                _db.SaveChanges();
                return RedirectToAction("List");
            }
            return NotFound();
        }
    }
}
//<b>bold</b>, <strong>bold</strong>
//<i>italic</i>, <em>italic</em>
//<u>underline</u>, <ins>underline</ins>
//<s>strikethrough</s>, <strike>strikethrough</strike>, <del>strikethrough</del>
//<b>bold <i>italic bold <s>italic bold strikethrough</s> <u>underline italic bold</u></i> bold</b>
//<a href="http://www.example.com/">inline URL</a>
//<a href="tg://user?id=123456789">inline mention of a user</a>
//<code>inline fixed-width code</code>
//<pre>pre-formatted fixed-width code block</pre>
//<pre><code class="language-python">pre-formatted fixed-width code block written in the Python programming language</code></pre>