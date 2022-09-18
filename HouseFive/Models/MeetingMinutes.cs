using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HouseFive.Models
{
    public class MeetingMinutes
    {
        public int Id { get; set; }
        public DateTime Title { get; set; }
        public string Article { get; set; }
        public string DocURl { get; set; } 
    }
}
