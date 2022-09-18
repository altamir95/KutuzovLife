using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HouseFive.ViewModels
{
    public class HouseAdviserViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Имя")]
        public string FirstName { get; set; }
        [Display(Name = "Фамилия")]
        public string LastName { get; set; }
        [Display(Name = "Аватар")]
        public IFormFile Avatar { get; set; }
        [Display(Name = "Должнасть")]
        public string Position { get; set; }
        [Display(Name = "Цитата")]
        public string Quote { get; set; }
    }
}
