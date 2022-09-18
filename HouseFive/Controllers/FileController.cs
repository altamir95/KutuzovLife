using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HouseFive.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class FileController : Controller
    {
        [HttpPost]
        public async Task<ActionResult> UploadImage(IFormFile upload)
        {
            if (upload.Length <= 0) return null; 

            var fileName = Guid.NewGuid() + Path.GetExtension(upload.FileName).ToLower();
             

            var filePath = Path.Combine(
                Directory.GetCurrentDirectory(), "wwwroot/articleimg",
                fileName);

            using (var stream = System.IO.File.Create(filePath))
            {
                await upload.CopyToAsync(stream);
            }  

            var sss = new
            {
                uploaded = 1,
                fileName = fileName,
                url = $"{"/articleimg/"}{fileName}",
                error = new
                {
                    message = "image is uploaded"
                }
            };

            return new ObjectResult(sss);
        }
    }
}
