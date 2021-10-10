using System;
using System.Collections.Generic;

namespace ELEKSUNI
{
    class Map
    {
        private Random randomizer;
        public delegate void MapHandler();
        public event MapHandler PlayerReachedExit;
        public static Dictionary<string, (int, int)> directionVectors = new Dictionary<string, (int, int)>()
        {
            { "Север", (-1, 0) } , { "Юг", (1, 0) }, { "Восток", (0, 1) }, { "Запад", (0, -1) }
        };
        public static Dictionary<(int, int), string> directionNames = new Dictionary<(int, int), string>()
        {
            { (-1, 0), "Север" } , { (1, 0), "Юг"}, { (0, 1), "Восток"}, { (0, -1), "Запад"}
        };
        private Dictionary<(int x, int y), Spot> spots;
        private DateTime currentTime, night, morning;
        private Player player;
        private Spot playerSpot;
        private Spot exit;
        public Map(Player player)
        {
            randomizer = new Random(DateTime.Now.Millisecond);
            spots = new Dictionary<(int x, int y), Spot>();
            currentTime = DateTime.Today.AddHours(12);
            night = DateTime.Today.AddHours(21);
            morning = DateTime.Today.AddHours(6);
            this.player = player;
            CreateNewMap((int)MainQuestConfig.MapSize, player);
        }
        public void AddSpot(Spot newSpot)
        {
            spots.Add(newSpot.Coordinates, newSpot);
        }
        public bool NotNightTime()
        {
            return ((currentTime.Hour >= morning.Hour) && (currentTime.Hour <= night.Hour));
        }
        public void SetPlayerLocation((int, int) index)
        {
            playerSpot = spots[index];
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
            input = playerSpot.GetAvailableDirections()[MainQuest.GetInput(playerSpot.GetAvailableDirections())];
            MainQuest.ClearOutdateInfo();
            if (!directionVectors.ContainsKey(input))
            {
                return $" ";
            }
            else if (!NotNightTime())
            {
                return $" слишком темно для перехода";
            }
            else if (player.GetPlayerStamina() < player.CalculateStaminaNeededToTravel())
            {
                return $" вы слишком устали, нужен отдых";
            }
            else
            {
                playerSpot = GetNearestSpotInDirection(playerSpot, input);
                ChangeTime(player.CalculateTimeNeededToTravel());
                player.Travel();
                if (playerSpot == this.exit)
                {
                    PlayerReachedExit?.Invoke();
                }
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
        private void CreateNewMap(int mapSize, Player player)
        {
            for (int i = 0; i <= mapSize; i++)
            {
                for (int j = 0; j <= mapSize; j++)
                {
                    this.AddSpot(new Spot((i, j), PickRandomDescription(GetSpotDescriptions())));
                }
            }
            CreateExit();
            CreateMaze();
        }
        private string PickRandomDescription(List<String> descriptions)
        {
            return descriptions[randomizer.Next(0, descriptions.Count - 1)];
        }
        public void CreateExit()
        {
            Dictionary<string, (int, int)> possibleExitLocations = new Dictionary<string, (int, int)>(directionVectors);
            Spot randomBorder = GetRandomBorderSpot();
            foreach (var direction in randomBorder.GetAvailableDirections())
            {
                possibleExitLocations.Remove(direction);
            }
            //picking random non existing direction
            (int, int)[] tempArray = new (int, int)[possibleExitLocations.Count];
            possibleExitLocations.Values.CopyTo(tempArray, 0);
            (int, int) chosenEscapeDirection = tempArray[randomizer.Next(0, tempArray.Length - 1)];
            this.exit = new Spot(AddCoordinates(randomBorder.Coordinates, chosenEscapeDirection),
                $" Дорога! Куда-то она да приведет. Вы выбрались!");
            this.AddSpot(exit);
            randomBorder.AddAvailableTravelDirection(directionNames[chosenEscapeDirection]);
        }
        private Spot GetRandomBorderSpot()
        {
            int x = randomizer.Next(0, (int)MainQuestConfig.MapSize);
            int y;
            if (x == 0 || x == (int)MainQuestConfig.MapSize)
            {
                y = randomizer.Next(0, (int)MainQuestConfig.MapSize);
            }
            else
            {
                y = randomizer.Next(0, 1) * (int)MainQuestConfig.MapSize;
            }
            return spots[(x, y)];
        }
        private Spot GetRandomSpot()
        {
            int x = randomizer.Next(0, (int)MainQuestConfig.MapSize);
            int y = randomizer.Next(0, (int)MainQuestConfig.MapSize);
            return spots[(x, y)];
        }
        public void CreateMaze()
        {
            int mazeWallCounter = 0;
            while(mazeWallCounter < (int)MainQuestConfig.MapSize * (int)MainQuestConfig.MazeDifficulty)
            {
                Spot baseSpot = GetRandomSpot();
                string direction = GetRandomAvailableDirection(baseSpot);
                Spot nextSpot = GetNearestSpotInDirection(baseSpot, direction);
                if (PossibleToSeparate(baseSpot, nextSpot))
                {
                    SeparateSpots(baseSpot, nextSpot, direction);
                    mazeWallCounter++;
                }
            }
        }

        private string GetRandomAvailableDirection(Spot baseSpot)
        {
            return baseSpot.GetAvailableDirections()[randomizer.Next(0, baseSpot.GetAvailableDirections().Count - 1)];
        }

        public static (int, int) AddCoordinates((int x, int y) coordinates1, (int x, int y) coordinates2)
        {
            return (coordinates1.x + coordinates2.x, coordinates1.y + coordinates2.y);
        }
        private List<string> GetSpotDescriptions()
        {
            List<string> descriptions = new List<string>();
            descriptions.Add("Просека. Много поваленных дереьев");
            descriptions.Add("Лес как лес. Кроме опавшей листвы ничего интересного");
            descriptions.Add("Старый дуб. Есть большое дупло, кажется, до него можно добраться");
            descriptions.Add("Смешанный лес. Много кустарника, есть ягоды");
            descriptions.Add("Преобладает хвоя приятно дышать полной грудью");
            descriptions.Add("Большая удача вы нашли ручей");
            descriptions.Add("Заросшее русло высохшей реки");
            descriptions.Add("Пещера. Выглядит довольно большой");
            descriptions.Add("Покинутая землянка. Дверь выглядит функционирующей, но замок рассыпался");
            descriptions.Add("Большая поляна");
            descriptions.Add("Овраг");
            return descriptions;
        }
        private bool PossibleToSeparate(Spot baseSpot, Spot nextSpot)
        {
            if ((baseSpot != exit) && (nextSpot != exit))
            {
                if (baseSpot.GetAvailableDirections().Count > 3 && nextSpot.GetAvailableDirections().Count > 3)
                {
                    return true;
                }
            }
            return false;
        }
        private Spot GetNearestSpotInDirection(Spot baseSpot, string direction)
        {
            (int, int) vector = directionVectors[direction];
            return spots[AddCoordinates(baseSpot.Coordinates, vector)];
        }
        private void SeparateSpots(Spot baseSpot, Spot nextSpot, string direction)
        {
            baseSpot.RemoveAvailableTravelDirection(direction);
            nextSpot.RemoveAvailableTravelDirection(GetOppositeDirection(direction));     
        }
        private string GetOppositeDirection(string direction)
        {
            (int, int) vector = directionVectors[direction];
            (int, int) inversVector = (vector.Item1 * -1, vector.Item2 * -1);
            return directionNames[inversVector];
        }
    }
}
