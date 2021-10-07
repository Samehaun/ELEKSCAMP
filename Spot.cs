using System;
using System.Collections.Generic;

namespace ELEKSUNI
{
    public class Spot
    {
        private List<string> travelDirecionsAvailableFromThisSpot;
        public string Description { get; set; }
        public (int x, int y) Coordinates { get;}
        public Spot((int i, int j) tuple, string description)
        {
            travelDirecionsAvailableFromThisSpot = new List<string>();
            SetAvailableTravelDirections(tuple);
            this.Coordinates = tuple;
            this.Description = description;
        }
        public void AddAvailableTravelDirection(String direction)
        {
            travelDirecionsAvailableFromThisSpot.Add(direction);
        }
        private void SetAvailableTravelDirections((int i, int j) tuple)
        {
            if (tuple.i > 0)
            {
                travelDirecionsAvailableFromThisSpot.Add("Север");
            }
            if (tuple.j > 0)
            {
                travelDirecionsAvailableFromThisSpot.Add("Запад");
            }
            if (tuple.i < (int)MainQuestConfig.MapSize)
            {
                travelDirecionsAvailableFromThisSpot.Add("Юг");
            }
            if (tuple.j < (int)MainQuestConfig.MapSize)
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
    }
}
