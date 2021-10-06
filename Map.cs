using System;
using System.Collections.Generic;
using System.Text;

namespace ELEKSUNI
{
    class Map
    {
        private Dictionary<(int x, int y), Spot> spots;
        private DateTime currentTime, night, morning;
        public Map()
        {
            spots = new Dictionary<(int x, int y), Spot>();
            currentTime = DateTime.Today.AddHours(12);
            night = DateTime.Today.AddHours(21);
            morning = DateTime.Today.AddHours(6);
        }
        public void TimeOfTheDay(double hours)
        {
            this.currentTime = currentTime.AddHours(hours);
        }
        public void AddSpot(Spot newSpot)
        {
            spots.Add(newSpot.Coordinates, newSpot);
        }
        public bool NotNightTime()
        {
            return ((currentTime.Hour >= morning.Hour) && (currentTime.Hour <= night.Hour));
        }
    }
}
