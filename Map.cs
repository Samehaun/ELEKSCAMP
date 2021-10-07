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
        private Player player;
        private Spot playerSpot;
        public Map(Player player)
        {
            spots = new Dictionary<(int x, int y), Spot>();
            currentTime = DateTime.Today.AddHours(12);
            night = DateTime.Today.AddHours(19);
            morning = DateTime.Today.AddHours(6);
            this.player = player;
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
        public void SetPlayerLocation((int, int) tupl)
        {
            playerSpot = spots[tupl];
        }
        public List<string> GetPossibleOptions()
        {
            return playerSpot.GetListOfPossibleOptions();
        }
        public string Travel()
        {
            string input;
            MainQuest.ShowAvailableOptions(playerSpot.GetAvailableDirections());
            input = playerSpot.GetListOfPossibleOptions()[MainQuest.GetConsoleInput(playerSpot.GetAvailableDirections())];
            if (!NotNightTime())
            {
                return $" уже слишком поздно для перехода";
            }
            else if (player.GetPlayerStamina() < CalculateStaminaNeededToTravel(player))
            {
                return $" вы слишком устали нужен отдых";
            }
            else
            {
                switch (input)
                {
                    case "Север":
                        playerSpot = spots[(playerSpot.Coordinates.x - 1, playerSpot.Coordinates.y)];
                        break;
                    case "Юг":
                        playerSpot = spots[(playerSpot.Coordinates.x + 1, playerSpot.Coordinates.y)];
                        break;
                    case "Запад":
                        playerSpot = spots[(playerSpot.Coordinates.x, playerSpot.Coordinates.y - 1)];
                        break;
                    case "Восток":
                        playerSpot = spots[(playerSpot.Coordinates.x, playerSpot.Coordinates.y + 1)];
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
            return CalculateTimeNeededToTravel(player) * (int)MainQuestConfig.BasePlayerStaminaConsuption;
        }
        public string GetLocationDescription()
        {
            return playerSpot.Description;
        }
    }
}
