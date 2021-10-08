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
        public static Dictionary<string, (int x, int y) Tuple> directions;
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
        public void AddSpot(Spot newSpot)
        {
            spots.Add(newSpot.Coordinates, newSpot);
        }
        public bool NotNightTime()
        {
            return ((currentTime.Hour >= morning.Hour) && (currentTime.Hour <= night.Hour));
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
            MainQuest.Output("Выберите направление");
            MainQuest.ShowAvailableOptions(playerSpot.GetAvailableDirections());
            input = playerSpot.GetAvailableDirections()[MainQuest.GetConsoleInput(playerSpot.GetAvailableDirections())];
            if (!NotNightTime())
            {
                return $" слишком темно для перехода";
            }
            else if (player.GetPlayerStamina() < player.CalculateStaminaNeededToTravel())
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
                    default:
                        MainQuest.ClearOutdateInfo();
                        return $" ";
                }
                ChangeTime(player.CalculateTimeNeededToTravel());
                player.Travel();
                MainQuest.ClearOutdateInfo();
                return $" Вы благополучно добрались до следующей зоны";
            }            
        }
        public string GetLocationDescription()
        {
            return playerSpot.Description;
        }
        public void ChangeTime(double timeSpent)
        {
            this.currentTime = currentTime.AddHours(timeSpent);
        }
    }
}
