using System;
using System.Collections.Generic;

namespace ELEKSUNI
{
    class Spot
    {
        private List<Keys> travelDirecionsAvailableFromThisSpot;
        public Keys Description { get; }
        public (int x, int y) Coordinates { get; private set; }
        public Spot(Keys description)
        {
            travelDirecionsAvailableFromThisSpot = new List<Keys>();
            this.Description = description;
        }
        public Spot((int, int) index, Keys description)
        {
            travelDirecionsAvailableFromThisSpot = new List<Keys>();
            SetAvailableTravelDirections(index);
            this.Coordinates = index;
            this.Description = description;
        }
        public void AddAvailableTravelDirection(Keys direction)
        {
            travelDirecionsAvailableFromThisSpot.Add(direction);
        }
        private void SetAvailableTravelDirections((int i, int j) index)
        {
            if (index.i > 0 && index.i <= (int)MainQuestConfig.MapSize)
            {
                travelDirecionsAvailableFromThisSpot.Add(Keys.North);
            }
            if (index.j > 0 && index.j <= (int)MainQuestConfig.MapSize)
            {
                travelDirecionsAvailableFromThisSpot.Add(Keys.West);
            }
            if (index.i < (int)MainQuestConfig.MapSize)
            {
                travelDirecionsAvailableFromThisSpot.Add(Keys.South);
            }
            if (index.j < (int)MainQuestConfig.MapSize)
            {
                travelDirecionsAvailableFromThisSpot.Add(Keys.East);
            }
        }
        public List<Keys> GetAvailableDirections()
        {
            List<Keys> options = new List<Keys>();
            options.AddRange(travelDirecionsAvailableFromThisSpot);
            options.Add(Keys.Cancel);
            return options;
        }
        public List<Keys> GetListOfPossibleOptions()
        {
            List<Keys> posibilities = new List<Keys>();
            posibilities.Add(Keys.Travel);
            posibilities.Add(Keys.Rest);
            posibilities.Add(Keys.Sleep);
            return posibilities;
        }
        public void RemoveAvailableTravelDirection(Keys direction)
        {
            this.travelDirecionsAvailableFromThisSpot.Remove(direction);
        }
        public void SetPosition((int, int) index)
        {
            Coordinates = index;
            SetAvailableTravelDirections(index);
        }
    }
}
