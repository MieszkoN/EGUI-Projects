using System;
using System.Collections.Generic;

namespace Calendar.Models {
    public class CalendarModel {
        public int actualYear;
        public int actualMonth;
        public string actualMonthWord;
        public int firstDayOfMonth;
        public int daysInMonth;
        public int daysInPreviousMonth;
        public List<string> daysWithEvents;

        public void setFirstDayOfMonth (int year, int month) {
            var firstDay = new DateTime(year, month, 1);
            switch(firstDay.DayOfWeek) {
                case DayOfWeek.Sunday:
                    firstDayOfMonth = 0;
                    break;

                case DayOfWeek.Monday:
                    firstDayOfMonth = 1;
                    break;

                case DayOfWeek.Tuesday:
                    firstDayOfMonth = 2;
                    break;

                case DayOfWeek.Wednesday:
                    firstDayOfMonth = 3;
                    break;

                case DayOfWeek.Thursday:
                    firstDayOfMonth = 4;
                    break;
                
                case DayOfWeek.Friday:
                    firstDayOfMonth = 5;
                    break;

                case DayOfWeek.Saturday:
                    firstDayOfMonth = 6;
                    break;

                default:
                    break;
            }
        }

        public string getPreviousMonth() {
            int month = actualMonth - 1;
            int year = actualYear;
            if (month == 0) {
                month = 12;
                year--;
            }
            string date = month.ToString() + "-" + year;

            return date;
        }

        public string getNextMonth() {
            int month = actualMonth + 1;
            int year = actualYear;
            if (month == 13) {
                month = 1;
                year++;
            }

            string date = month.ToString() + "-" + year;

            return date;
        }

        

    }
}