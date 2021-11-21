using System;
using System.Collections.Generic;

namespace ELEKSUNI
{
    class Map
    {
        public Prefabs prefabs;
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
        private Player player;
        public Spot PlayerSpot { get; private set; }
        private Spot exit;
        public bool ExitReached { get; private set; }
        public Map(Player player)
        {
            prefabs = new Prefabs();
            randomizer = new Random(DateTime.Now.Millisecond);
            map = new Dictionary<(int x, int y), Spot>();
            prefabs.GenerateSpotPrefabs();
            spots = new List<Spot>(prefabs.GetPrefabs());
            this.player = player;
            player.Inventory.Add(prefabs.simpleClothes);
            player.CurrentClothes = prefabs.simpleClothes;
            ExitReached = false;
            if (player.Name == "Test" || player.Name == "test")
            {
                player.Inventory.AddMoney(500);
                CreateTestMap();
            }
            else
            {
                CreateNewMap((int)MainQuestConfig.MapSize);
            }
        }
        private void AddSpot(Spot newSpot)
        {
            map.Add(newSpot.Coordinates, newSpot);
        }
        public void SetPlayerLocation((int, int) index)
        {
            PlayerSpot = map[index];
        }
        private void CreateNewMap(int mapSize)
        {
            for (int i = 0; i <= mapSize; i++)
            {
                for (int j = 0; j <= mapSize; j++)
                {
                    Spot nextSpot = PickRandomSpotFromSpotPrefabs(spots);
                    nextSpot.SetPosition((i, j));
                    AddSpot(nextSpot);
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
            while (mazeWallCounter < (int)MainQuestConfig.MapSize * (int)MainQuestConfig.MazeDifficulty)
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
        public Keys GetRandomAvailableDirection(Spot baseSpot)
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
                if (baseSpot.GetAvailableDirections().Count > 2 && nextSpot.GetAvailableDirections().Count > 2)
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
            (int, int) inverseVector = (vector.Item1 * -1, vector.Item2 * -1);
            return directionNames[inverseVector];
        }
        public void Go(Keys direction)
        {
            PlayerSpot = GetNearestSpotInDirection(PlayerSpot, direction);
            if (PlayerSpot == exit)
            {
                ExitReached = true;
            }
        }
        private void CreateTestMap()
        {
            prefabs.GenerateTestSpotPrefabs();
            List<Spot> spotsToTest = prefabs.GetPrefabs();
            int n = 0;
            for (int i = 0; i <= 3; i++)
            {
                for (int j = 0; j <= 3; j++)
                {
                    spotsToTest[n].SetPosition((i, j));
                    AddSpot(spotsToTest[n]);
                    n++;
                }
            }
            exit = new Spot((0, -1), Keys.Exit);
            map[(0, 0)].AddAvailableTravelDirection(Keys.West);
            AddSpot(exit);
        }
    }
}