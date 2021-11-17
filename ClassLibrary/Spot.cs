using System;
using System.Collections.Generic;

namespace ELEKSUNI
{
    class Spot
    {
        private List<Keys> travelDirecionsAvailableFromThisSpot;
        public Keys Description { get; }
        public bool searched;
        public (int x, int y) Coordinates { get; private set; }
        public Item item;
        public NPC npc;
        public Spot(Keys description)
        {
            travelDirecionsAvailableFromThisSpot = new List<Keys>();
            this.Description = description;
            searched = false;
        }
        public Spot((int, int) index, Keys description) : this(description)
        {
            SetAvailableTravelDirections(index);
            this.Coordinates = index;
        }
        public Spot(Keys description, Item hidden = null, NPC npc = null) : this(description)
        {
            item = hidden;
            this.npc = npc;
        }
        public Spot(Keys description, NPC npc = null) : this(description, null, npc) {}
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
            return options;
        }
        public List<Keys> GetListOfPossibleOptions()
        {
            List<Keys> posibilities = new List<Keys>();
            posibilities.Add(Keys.Travel);
            posibilities.Add(Keys.Rest);
            posibilities.Add(Keys.Sleep);
            posibilities.Add(Keys.Search);
            posibilities.Add(Keys.Inventory);
            if(npc != null && npc.IsHostile)
            {
                posibilities.Clear();
                posibilities.Add(Keys.Fight);
                posibilities.Add(Keys.Run);
            }
            else if(npc != null && npc.Health > 0)
            {
                posibilities.Add(Keys.NPC);
            }
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