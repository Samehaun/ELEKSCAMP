using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ELEKSUNI
{
    class NPC
    {
        public Keys Name { get; private set; }
        public int Healt { get; set; }
        public int Defence { get; private set; }
        public int Attack { get; private set; }
        public Inventory inventory;
        public bool IsHostile { get; private set; }
        public List<Keys> GetListOfPossibleOptions()
        {
            List<Keys> posibilities = new List<Keys>();
            if (IsHostile)
            {
                posibilities.Add(Keys.Fight);
                posibilities.Add(Keys.Run);
            }
            else
            {
                posibilities.Add(Keys.Trade);
                posibilities.Add(Keys.Buy);
                posibilities.Add(Keys.Sell);
                posibilities.Add(Keys.Steal);
                posibilities.Add(Keys.Cancel);
            }
            return posibilities;
        }
        public NPC(Keys name, int health, int defence, int attack, bool isHostile, List<Item> items = null)
        {
            Name = name;
            Healt = health;
            Defence = defence;
            Attack = attack;
            IsHostile = isHostile;
            inventory = new Inventory();
            if(items != null)
            {
                inventory.Items.AddRange(items);
            }
        }

    }
}
