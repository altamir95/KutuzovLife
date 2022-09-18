using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HouseFive.Models
{
    public class InitiativeAppeal
    {
        public int InitiativeId { get; set; }
        public Initiative Initiative { get; set; }
        public int AppealId { get; set; }
        public Appeal Appeal { get; set; }
    }
}
