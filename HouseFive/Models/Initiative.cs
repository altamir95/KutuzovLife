using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HouseFive.Models
{
    public class Initiative
    {
        public int Id { get; set; }
        public string Article { get; set; }
        public InitiativeAppeal InitiativeAppeal { get; set; }

    }
}
