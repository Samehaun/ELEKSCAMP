using System;

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
        public void Load(TimeSave time)
        {
            currentTime = time.Now;
            night = time.Night;
            morning = time.Morning;
        }
        public TimeSave Save()
        {
            return new TimeSave(currentTime, night, morning);
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
    struct TimeSave
    {
        public DateTime Now { get; set; }
        public DateTime Night { get; set; }
        public DateTime Morning { get; set; }
        public TimeSave(DateTime current, DateTime night, DateTime morning)
        {
            Now = current;
            Night = night;
            Morning = morning;
        }
    }
}