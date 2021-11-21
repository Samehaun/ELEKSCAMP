using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ELEKSUNI
{
    class Time
    {
        private DateTime currentTime, night, morning;
        public Time()
        {
            currentTime = DateTime.Today.AddHours(12);
            night = DateTime.Today.AddHours(21);
            morning = DateTime.Today.AddHours(6);
        }
        public Time(DateTime time)
        {
            currentTime = time;
            night = time.Date.AddHours(21);
            morning = time.Date.AddHours(6);
        }
        public bool DayTime()
        {
            return ((currentTime.Hour >= morning.Hour) && (currentTime.Hour <= night.Hour));
        }
        public void ChangeTime(double timeSpent)
        {
            this.currentTime = currentTime.AddHours(timeSpent);
        }
    }
}