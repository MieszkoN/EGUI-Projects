using System;
using System.Collections.Generic;

namespace Calendar.Models {
    public class SingleDayModel {
        public int yearOfSingleDay;
        public int monthOfSingleDay;
        public int dayOfSingleDay;
        public List<SingleEventModel> eventsDuringTheDay;


        public string dateOfSingleDay {
            get {
                return dayOfSingleDay.ToString()+ "-" + monthOfSingleDay.ToString()+ "-" + yearOfSingleDay.ToString();
            }
        }
    }
}