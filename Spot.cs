using System;
using System.Collections.Generic;
using System.Text;

namespace ELEKSUNI
{
    public class Spot
    {
        public string Description { get; set; }
        public (int x, int y) Coordinates { get;}
        public Item HiddenObject { get; set; }
        public NPC NPC { get; set; }
        public Spot((int i, int j) tuple, string description, Item treasure)
        {
            this.Coordinates = tuple;
            this.Description = description;
            this.HiddenObject = treasure;
        }
        public void GetHiddenObject(Player player)
        {
            HiddenObject.PickAnItem(player);
        }
    }
}
