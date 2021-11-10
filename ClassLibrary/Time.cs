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
        public bool NotNightTime()
        {
            return ((currentTime.Hour >= morning.Hour) && (currentTime.Hour <= night.Hour));
        }
        public void ChangeTime(double timeSpent)
        {
            this.currentTime = currentTime.AddHours(timeSpent);
        }
    }
}