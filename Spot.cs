using System;
using System.Collections.Generic;

namespace ELEKSUNI
{
    public class Spot
    {
        private List<string> travelDirecionsAvailableFromThisSpot;
        public string Description { get; set; }
        public (int x, int y) Coordinates { get;}
        public Item HiddenObject { get; set; }
        public NPC NPC { get; set; }
        public Spot((int i, int j) tuple, string description, Item treasure, NPC npc)
        {
            travelDirecionsAvailableFromThisSpot = new List<string>();
            SetAvailableTravelDirections(tuple);
            this.Coordinates = tuple;
            this.Description = description;
            this.HiddenObject = treasure;
            this.NPC = npc;
        }
        public void GetHiddenObject(Player player)
        {
            HiddenObject.PickThisItem(player);
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
        public bool IsPossibleToMoveInThatDurection(string direction)
        {
            return travelDirecionsAvailableFromThisSpot.Contains(direction);
        }
    }
}
