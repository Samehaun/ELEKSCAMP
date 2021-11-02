using System;
using System.Collections.Generic;

namespace ELEKSUNI
{
    class Map
    {
        private Prefabs prefabs;
        private Random randomizer;
        public static Dictionary<Keys, (int, int)> directionVectors = new Dictionary<Keys, (int, int)>()
        {
            { Keys.North, (-1, 0) } , { Keys.South, (1, 0) }, { Keys.East, (0, 1) }, { Keys.West, (0, -1) }
        };
        public static Dictionary<(int, int), Keys> directionNames = new Dictionary<(int, int), Keys>()
        {
            { (-1, 0), Keys.North } , { (1, 0), Keys.South}, { (0, 1), Keys.East}, { (0, -1), Keys.West}
        };
        private List<Spot> spots;
        private Dictionary<(int x, int y), Spot> map;
        private DateTime currentTime, night, morning;
        private Player player;
        private Spot playerSpot;
        private Spot exit;
        public bool ExitReached { get; private set; }
        public Map(Player player)
        {
            prefabs = new Prefabs();
            randomizer = new Random(DateTime.Now.Millisecond);
            map = new Dictionary<(int x, int y), Spot>();
            spots = new List<Spot>(prefabs.GetPrefabs());
            currentTime = DateTime.Today.AddHours(12);
            night = DateTime.Today.AddHours(21);
            morning = DateTime.Today.AddHours(6);
            this.player = player;
            CreateNewMap((int)MainQuestConfig.MapSize, player);
            ExitReached = false;
        }
        private void AddSpot(Spot newSpot)
        {
            map.Add(newSpot.Coordinates, newSpot);
        }
        public bool NotNightTime()
        {
            return ((currentTime.Hour >= morning.Hour) && (currentTime.Hour <= night.Hour));
        }
        public void SetPlayerLocation((int, int) index)
        {
            playerSpot = map[index];
        }
        public List<Keys> GetPossibleOptions()
        {
            return playerSpot.GetListOfPossibleOptions();
        }
        public Keys Travel(Keys input)
        {
            if (!directionVectors.ContainsKey(input))
            {
                return playerSpot.Description;
            }
            else if (!NotNightTime())
            {
                return Keys.NightTime;
            }
            else if (player.GetPlayerStamina() < player.CalculateStaminaNeededToTravel())
            {
                return Keys.RestNeeded;
            }
            else
            {
                playerSpot = GetNearestSpotInDirection(playerSpot, input);
                ChangeTime(player.CalculateTimeNeededToTravel());
                player.RecaculateStateDueToTraveling();
                if (playerSpot == this.exit)
                {
                    ExitReached = true;
                }
                return Keys.NextZone;
            }
        }
        public Keys GetLocationDescription()
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
                    Spot nextSpot = PickRandomSpotFromSpotPrefabs(spots);
                    nextSpot.SetPosition((i, j));
                    this.AddSpot(nextSpot);
                }
            }
            CreateExit();
            CreateMaze();
        }
        private Spot PickRandomSpotFromSpotPrefabs(List<Spot> spots)
        {
            Spot random = spots[randomizer.Next(0, spots.Count - 1)];
            spots.Remove(random);
            return random;
        }
        private void CreateExit()
        {
            Dictionary<Keys, (int, int)> possibleExitLocations = new Dictionary<Keys, (int, int)>(directionVectors);
            Spot randomBorder = GetRandomBorderSpot();
            foreach (var direction in randomBorder.GetAvailableDirections())
            {
                possibleExitLocations.Remove(direction);
            }
            //picking random non existing direction
            (int, int)[] tempArray = new (int, int)[possibleExitLocations.Count];
            possibleExitLocations.Values.CopyTo(tempArray, 0);
            (int, int) chosenEscapeDirection = tempArray[randomizer.Next(0, tempArray.Length - 1)];
            this.exit = new Spot(AddCoordinates(randomBorder.Coordinates, chosenEscapeDirection), Keys.Exit);
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
            return map[(x, y)];
        }
        private Spot GetRandomSpotOnTheMap()
        {
            int x = randomizer.Next(0, (int)MainQuestConfig.MapSize);
            int y = randomizer.Next(0, (int)MainQuestConfig.MapSize);
            return map[(x, y)];
        }
        private void CreateMaze()
        {
            int mazeWallCounter = 0;
            while(mazeWallCounter < (int)MainQuestConfig.MapSize * (int)MainQuestConfig.MazeDifficulty)
            {
                Spot baseSpot = GetRandomSpotOnTheMap();
                Keys direction = GetRandomAvailableDirection(baseSpot);
                Spot nextSpot = GetNearestSpotInDirection(baseSpot, direction);
                if (PossibleToSeparate(baseSpot, nextSpot))
                {
                    SeparateSpots(baseSpot, nextSpot, direction);
                    mazeWallCounter++;
                }
            }
        }
        private Keys GetRandomAvailableDirection(Spot baseSpot)
        {
            return baseSpot.GetAvailableDirections()[randomizer.Next(0, baseSpot.GetAvailableDirections().Count - 1)];
        }
        public static (int, int) AddCoordinates((int x, int y) coordinates1, (int x, int y) coordinates2)
        {
            return (coordinates1.x + coordinates2.x, coordinates1.y + coordinates2.y);
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
        private Spot GetNearestSpotInDirection(Spot baseSpot, Keys direction)
        {
            (int, int) vector = directionVectors[direction];
            return map[AddCoordinates(baseSpot.Coordinates, vector)];
        }
        private void SeparateSpots(Spot baseSpot, Spot nextSpot, Keys direction)
        {
            baseSpot.RemoveAvailableTravelDirection(direction);
            nextSpot.RemoveAvailableTravelDirection(GetOppositeDirection(direction));     
        }
        private Keys GetOppositeDirection(Keys direction)
        {
            (int, int) vector = directionVectors[direction];
            (int, int) inversVector = (vector.Item1 * -1, vector.Item2 * -1);
            return directionNames[inversVector];
        }
        public List<Keys> GetTravelDirections()
        {
            return playerSpot.GetAvailableDirections();
        }


    }
}
