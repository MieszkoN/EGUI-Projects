using System;
using System.Collections.Generic;

namespace Calendar.Models {
    public class SingleEventModel {
        public string timeOfEvent;
        public string descriptionOfEvent;
        public int yearOfSingleEvent;
        public int monthOfSingleEvent;
        public int dayOfSingleEvent;
        public int idEvent;
        public string dateOfSingleEvent {
            get {
                return dayOfSingleEvent.ToString()+ "-" + monthOfSingleEvent.ToString()+ "-" + yearOfSingleEvent.ToString();
            }
        }

        public SingleEventModel() {}
    }
}