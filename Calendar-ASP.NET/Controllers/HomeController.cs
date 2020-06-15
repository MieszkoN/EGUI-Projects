using System;
using System.Globalization;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.IO;
using Calendar.Models;

namespace Calendar.Controllers
{
    public class HomeController : Controller
    {
        private List<SingleEventModel> ev = new List<SingleEventModel>();
    
        
        //Get current year and month
        public IActionResult Index()
        {
            int year = DateTime.Today.Year;
            int month = DateTime.Today.Month;
            return Index(year, month);
        }

        //Show Calendar
        [Route("{month:int}-{year:int}")]
        public IActionResult Index(int year, int month)
        {
            CalendarModel cal = new CalendarModel();
            cal.actualYear = year;
            cal.actualMonth = month;
            cal.actualMonthWord = new CultureInfo("en-US").DateTimeFormat.MonthNames[cal.actualMonth-1];
            cal.daysInMonth = DateTime.DaysInMonth(year, month);
            cal.setFirstDayOfMonth(year, month);
		    char[] separator = {'-'}; 
		    String[] strlist = cal.getPreviousMonth().Split(separator); 
		    int m = Int32.Parse(strlist[0]);
		    int y = Int32.Parse(strlist[1]);
            cal.daysInPreviousMonth = DateTime.DaysInMonth(y, m);
            readFromFile();
            cal.daysWithEvents = new List<string>();
            foreach (SingleEventModel sem in ev) {
                cal.daysWithEvents.Add(sem.dateOfSingleEvent);
            }

            return View(cal);
        }


        //Show screen of a single day
        [HttpGet]
        [Route("{day:int}-{month:int}-{year:int}")]
        public IActionResult SingleDayView(int year, int month, int day)
        {
            SingleDayModel singleDay = new SingleDayModel();
            singleDay.yearOfSingleDay = year;
            singleDay.monthOfSingleDay = month;
            singleDay.dayOfSingleDay = day;
            singleDay.eventsDuringTheDay = new List<SingleEventModel>();
            readFromFile();
            foreach (SingleEventModel sem in ev) {
                if(sem.dateOfSingleEvent == singleDay.dateOfSingleDay) {
                    singleDay.eventsDuringTheDay.Add(sem);
                }
            }
            return View(singleDay);
        }

        
        //show screen of adding an event
        [HttpGet]
        [Route("{day:int}-{month:int}-{year:int}-newEvent")]
        public IActionResult SingleEventView(int year, int month, int day)
        {
            SingleEventModel singleEvent = new SingleEventModel();
            singleEvent.yearOfSingleEvent = year;
            singleEvent.monthOfSingleEvent = month;
            singleEvent.dayOfSingleEvent = day;
            return View(singleEvent);
        }


        //add event in the single event screen
        [HttpPost]
        [Route("{day:int}-{month:int}-{year:int}-newEvent")]
        public IActionResult SingleEventView(string description, string timeOfEvent, int year, int month, int day) {
            SingleEventModel sem = new SingleEventModel();
            sem.descriptionOfEvent = description;
            sem.dayOfSingleEvent = day;
            sem.monthOfSingleEvent = month;
            sem.yearOfSingleEvent = year;
            sem.timeOfEvent = timeOfEvent;
            ev.Add(sem);
            readFromFile();
            saveToFile();
            return RedirectToAction("SingleDayView", new { year = year, month = month, day = day });
        }


        //Delete event in the single day screen or redirect to editing the event
        [HttpPost]
        [Route("{day:int}-{month:int}-{year:int}")]
        public IActionResult SingleDayView(string action, int id, int year, int month, int day) {
            if(action == "Delete") {
                SingleDayModel singleDay = new SingleDayModel();
            singleDay.yearOfSingleDay = year;
            singleDay.monthOfSingleDay = month;
            singleDay.dayOfSingleDay = day;
            singleDay.eventsDuringTheDay = new List<SingleEventModel>();
            readFromFile();
            foreach (SingleEventModel sem in ev) {
                if(sem.dateOfSingleEvent == singleDay.dateOfSingleDay) {
                    singleDay.eventsDuringTheDay.Add(sem);
                }
            }

            for(int i = 0;i<ev.Count(); i++) {
                if(ev[i].dayOfSingleEvent == day && ev[i].monthOfSingleEvent == month && ev[i].yearOfSingleEvent == year && ev[i].timeOfEvent == singleDay.eventsDuringTheDay[id].timeOfEvent && ev[i].descriptionOfEvent == singleDay.eventsDuringTheDay[id].descriptionOfEvent) {
                    ev.RemoveAt(i);
                }
            }
            saveToFile();
                return RedirectToAction("SingleDayView", new { year = year, month = month, day = day});
            } else {
                return RedirectToAction("SingleEventViewEdit", new { year = year, month = month, day = day, id = id});
            }
            
            
        }


        //show screen of editing an event
        [HttpGet]
        [Route("{day:int}-{month:int}-{year:int}-editEvent")]
        public IActionResult SingleEventViewEdit(int year, int month, int day, int id)
        {
            SingleEventModel singleEvent = new SingleEventModel();
            singleEvent.yearOfSingleEvent = year;
            singleEvent.monthOfSingleEvent = month;
            singleEvent.dayOfSingleEvent = day;
            singleEvent.idEvent = id;
            readFromFile();
            SingleDayModel sdm = new SingleDayModel();
            sdm.eventsDuringTheDay = new List<SingleEventModel>();
            foreach (SingleEventModel sem in ev) {
                if(sem.dateOfSingleEvent == singleEvent.dateOfSingleEvent) {
                    sdm.eventsDuringTheDay.Add(sem);
                }
            }
            singleEvent.descriptionOfEvent = sdm.eventsDuringTheDay[id].descriptionOfEvent;
            singleEvent.timeOfEvent = sdm.eventsDuringTheDay[id].timeOfEvent;
            return View(singleEvent);
        }

        //edit event in the single event screen
        [HttpPost]
        [Route("{day:int}-{month:int}-{year:int}-editEvent")]
        public IActionResult SingleEventViewEdit(string description, string timeOfEvent, int year, int month, int day) {
            SingleEventModel sem = new SingleEventModel();
            sem.descriptionOfEvent = description;
            sem.dayOfSingleEvent = day;
            sem.monthOfSingleEvent = month;
            sem.yearOfSingleEvent = year;
            sem.timeOfEvent = timeOfEvent;
            readFromFile();
            SingleDayModel sdm = new SingleDayModel();
            sdm.eventsDuringTheDay = new List<SingleEventModel>();
            foreach (SingleEventModel item in ev) {
                if(item.dateOfSingleEvent == sem.dateOfSingleEvent) {
                    sdm.eventsDuringTheDay.Add(item);
                }
            }
            for(int i = 0; i < ev.Count(); i++) {
                if(ev[i].dayOfSingleEvent == day && ev[i].monthOfSingleEvent == month && ev[i].yearOfSingleEvent == year && ev[i].timeOfEvent == sdm.eventsDuringTheDay[sem.idEvent].timeOfEvent && ev[i].descriptionOfEvent == sdm.eventsDuringTheDay[sem.idEvent].descriptionOfEvent) {
                    ev[i].timeOfEvent = timeOfEvent;
                    ev[i].descriptionOfEvent = description;
                }
            }
            
            saveToFile();
            return RedirectToAction("SingleDayView", new { year = year, month = month, day = day });
        }
        

        //read all events from file
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
                        SingleEventModel oneEvent = new SingleEventModel();
		                string[] strlist = line.Split(separator,  StringSplitOptions.RemoveEmptyEntries);
                        oneEvent.dayOfSingleEvent = Int32.Parse(strlist[0]);
                        oneEvent.monthOfSingleEvent = Int32.Parse(strlist[1]);
                        oneEvent.yearOfSingleEvent = Int32.Parse(strlist[2]);
                        oneEvent.timeOfEvent = strlist[3].Substring(0, 5);
                        oneEvent.descriptionOfEvent = strlist[3].Substring(5);
                        ev.Add(oneEvent);
                    }
                }
                ev.Sort(delegate(SingleEventModel sem1, SingleEventModel sem2)
                {
                    return sem1.timeOfEvent.CompareTo(sem2.timeOfEvent);
                });

            }
            catch (Exception) {
                throw new IOException("There is an error during reading the data from the file.");
            }
        }


        //save all events to the txt file
        public void saveToFile() {
             try {
                using (StreamWriter streamW = new System.IO.StreamWriter("myFile.txt")) {
                    foreach (SingleEventModel s in ev) {
                        streamW.WriteLine(s.dayOfSingleEvent.ToString()+ "@#%*!" +s.monthOfSingleEvent.ToString()+ "@#%*!" +s.yearOfSingleEvent.ToString() + "@#%*!"+s.timeOfEvent+s.descriptionOfEvent);
                    }
                }
            }
            catch (Exception) {
                throw new IOException("There is an error during saving the data to the file.");
            }
        }

    }
}
