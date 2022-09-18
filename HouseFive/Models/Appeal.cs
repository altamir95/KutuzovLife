using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HouseFive.Models
{
    public class Appeal
    {
        public int Id { get; set; }
        public string Author { get; set; }
        public string Title { get; set; }
        public string Article { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime CreationDate { get; set; }

        public InitiativeAppeal InitiativeAppeal { get; set; }
    }
}
