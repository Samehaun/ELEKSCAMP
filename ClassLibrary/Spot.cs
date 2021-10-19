using System;
using System.Collections.Generic;

namespace ELEKSUNI
{
    class Spot
    {
        private List<string> travelDirecionsAvailableFromThisSpot;
        public string Description { get; }
        public (int x, int y) Coordinates { get; private set; }
        public Spot(string description)
        {
            travelDirecionsAvailableFromThisSpot = new List<string>();
            this.Description = description;
        }
        public Spot((int, int) index, string description)
        {
            travelDirecionsAvailableFromThisSpot = new List<string>();
            SetAvailableTravelDirections(index);
            this.Coordinates = index;
            this.Description = description;
        }
        public void AddAvailableTravelDirection(string direction)
        {
            travelDirecionsAvailableFromThisSpot.Add(direction);
        }
        private void SetAvailableTravelDirections((int i, int j) index)
        {
            if (index.i > 0 && index.i <= (int)MainQuestConfig.MapSize)
            {
                travelDirecionsAvailableFromThisSpot.Add("Север");
            }
            if (index.j > 0 && index.j <= (int)MainQuestConfig.MapSize)
            {
                travelDirecionsAvailableFromThisSpot.Add("Запад");
            }
            if (index.i < (int)MainQuestConfig.MapSize)
            {
                travelDirecionsAvailableFromThisSpot.Add("Юг");
            }
            if (index.j < (int)MainQuestConfig.MapSize)
            {
                travelDirecionsAvailableFromThisSpot.Add("Восток");
            }
        }
        public List<string> GetAvailableDirections()
        {
            List<string> options = new List<string>();
            options.AddRange(travelDirecionsAvailableFromThisSpot);
            options.Add(" назад ");
            return options;
        }
        public List<string> GetListOfPossibleOptions()
        {
            List<string> posibilities = new List<string>();
            posibilities.Add("Идти");
            posibilities.Add("Отдыхать");
            posibilities.Add("Спать");
            return posibilities;
        }
        public void RemoveAvailableTravelDirection(string direction)
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
