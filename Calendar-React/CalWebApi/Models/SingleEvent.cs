using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CalendarWebApi.Models
{
    public class SingleEvent {
        public int id {get; set;}
        public string dateOfEvent {get; set;}
        public string timeOfEvent{ get; set; }
        public string description{ get; set; }

    }

}
 