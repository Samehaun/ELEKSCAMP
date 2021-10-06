using System;
using System.Collections.Generic;

namespace ELEKSUNI
{
    enum Directions
    {
        Север,
        Юг,
        Восток,
        Запад
    }
    class Map
    {
        private Dictionary<(int x, int y), Spot> spots;
        private DateTime currentTime, night, morning;
        public Map()
        {
            spots = new Dictionary<(int x, int y), Spot>();
            currentTime = DateTime.Today.AddHours(12);
            night = DateTime.Today.AddHours(19);
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
        public Spot GetSpotByCoordinate((int x, int y) tupl)
        {
            return spots[tupl];
        }
        public string Travel(Player player, string direction)
        {
            if (!NotNightTime())
            {
                return $" уже слишком поздно для перехода";
            }
            else if (player.GetPlayerStamina() < CalculateStaminaNeededToTravel(player))
            {
                return $" нужен отдых";
            }
            else if (!player.currentSpot.IsPossibleToMoveInThatDurection(direction))
            {
                return $" похоже, что в этом направлении невозможно пройти";
            }
            else
            {
                switch (direction)
                {
                    case "Север":
                        player.currentSpot = spots[(player.currentSpot.Coordinates.x - 1, player.currentSpot.Coordinates.y)];
                        break;
                    case "Юг":
                        player.currentSpot = spots[(player.currentSpot.Coordinates.x + 1, player.currentSpot.Coordinates.y)];
                        break;
                    case "Запад":
                        player.currentSpot = spots[(player.currentSpot.Coordinates.x, player.currentSpot.Coordinates.y - 1)];
                        break;
                    case "Восток":
                        player.currentSpot = spots[(player.currentSpot.Coordinates.x, player.currentSpot.Coordinates.y + 1)];
                        break;
                }
                return $" Вы благополучно добрались до следующей зоны";
            }
                
        }
        private double CalculateTimeNeededToTravel(Player player)
        {
            double time = (int)MainQuestConfig.BaseTimeToChangeLocation * ((int)MainQuestConfig.BasePlayerSpeed / player.GetPlayerSpeed());
            return time;
        }
        private double CalculateStaminaNeededToTravel(Player player)
        {
            return CalculateTimeNeededToTravel(player) * (int)MainQuestConfig.BasePlayerStaminaConsuption * player.OverweightFactor();
        }
    }
}
