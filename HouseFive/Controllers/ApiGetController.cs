using HouseFive.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HouseFive.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class ApiGetController : ControllerBase
    {
        private readonly HomeFiveContext _db;

        public ApiGetController(HomeFiveContext context, IConfiguration configuration)
        {
            _db = context; 
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        [HttpGet ]
        public bool Coupon(string code)
        {
            if (code == null) return    false ;
            string coupon = "СОСЕД2021";
            if (code.ToUpper() == coupon.ToUpper())
            {
                return   true ;
            }
            else
            {
                return  false ; 
            } 
        }
    }
}
