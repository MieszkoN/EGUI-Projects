using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.IO;
using CalendarWebApi.Models;
using System.Data;
using System.Web;
using System.Net.Http;
using System.Net;
using System.Text;

namespace CalendarWebApi.Controllers
{
    [ApiController]
    [Route("api/allEvents")]
    public class DayEvents : ControllerBase{
        public List<SingleEvent> allEvents = new List<SingleEvent>();

        

        public void readFromFile() {
            if (!System.IO.File.Exists("myFile.txt")) {
                using (System.IO.File.Create("myFile.txt")) {}
                return;
            }

            try {
                 using (StreamReader streamR = new StreamReader("myFile.txt")) {
                    string line;
                    string[] separator = {"@#%*!"}; 
                    while ((line = streamR.ReadLine()) != null) {
                        SingleEvent oneEvent = new SingleEvent();
		                string[] strlist = line.Split(separator,  StringSplitOptions.RemoveEmptyEntries);
                        oneEvent.id = Int32.Parse(strlist[0]);
                        oneEvent.dateOfEvent = strlist[1];
                        oneEvent.timeOfEvent = strlist[2];
                        oneEvent.description = strlist[3];
                        allEvents.Add(oneEvent);
                    }
                    allEvents.Sort(delegate(SingleEvent s1, SingleEvent s2)
                    {
                        return s1.timeOfEvent.CompareTo(s2.timeOfEvent);
                    });

                 }
            } catch (Exception) {
                throw new IOException("There is an error during reading the data from the file.");
            }
        }

        public void saveToFile() {
            if (!System.IO.File.Exists("myFile.txt")) {
                using (System.IO.File.Create("myFile.txt")) {}
                return;
            }

            try {
                using (StreamWriter streamW = new System.IO.StreamWriter("myFile.txt")) {
                    foreach (SingleEvent s in allEvents) {
                        streamW.WriteLine(s.id.ToString()+"@#%*!"+ s.dateOfEvent.ToString() + "@#%*!"+ s.timeOfEvent.ToString()+ "@#%*!" +s.description.ToString());
                    }
                }
            }
            catch (Exception) {
                throw new IOException("There is an error during saving the data to the file.");
            }
        }

        [HttpGet]
        public IEnumerable<SingleEvent> Get()
        {
            readFromFile();
            return allEvents;
        }

        [HttpPost]
        [ActionName("POST")]
        public SingleEvent Post([FromBody]SingleEvent ev)
        {
            readFromFile();
            ev.id = allEvents.Count();
            allEvents.Add(ev);
            saveToFile();
            return ev;
        }


        [HttpPut]
        [ActionName("PUT")]
        public SingleEvent Put([FromBody]SingleEvent ev)
        {
            readFromFile();
            foreach(SingleEvent se in allEvents) {
                if(ev.id == se.id) {
                    se.timeOfEvent = ev.timeOfEvent;
                    se.description = ev.description;
                    break;
                }
            }
            saveToFile();
            return ev;
        }

        [HttpDelete]
        [ActionName("DELETE")]
        public string Delete([FromBody]SingleEvent ev)
        {
            readFromFile();
            for(int i = 0; i < allEvents.Count(); i++) {
                if(allEvents[i].id == ev.id) {
                    allEvents.RemoveAt(i);
                    break;
                }
            }

            foreach(SingleEvent se in allEvents) {
                if(se.id > ev.id) {
                    se.id--;
                }
            }
            saveToFile();
            return "Deleted";
        }

    
      
    }
}
