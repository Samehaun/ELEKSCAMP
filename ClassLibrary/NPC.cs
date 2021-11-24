using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ELEKSUNI
{
    class NPC
    {
        public Keys Name { get; private set; }
        public int Health { get; private set; }
        public int Defence { get; private set; }
        public int Attack { get; private set; }
        public Inventory Inventory { get; private set; };
        public bool IsHostile { get; set; }
        public List<Keys> GetListOfPossibleOptions()
        {
            List<Keys> posibilities = new List<Keys>();
            if(Health < 0)
            {
                posibilities.Add(Keys.Loot);
                posibilities.Add(Keys.Cancel);
            }
            else if (IsHostile)
            {
                posibilities.Add(Keys.Fight);
                posibilities.Add(Keys.Run);
            }
            else
            {
                posibilities.Add(Keys.Trade);
                posibilities.Add(Keys.Steal);
                posibilities.Add(Keys.Fight);
                posibilities.Add(Keys.Cancel);
            }
            return posibilities;
        }
        public NPC(Keys name, int health, int defence, int attack, bool isHostile, List<Item> items = null)
        {
            Name = name;
            Health = health;
            Defence = defence;
            Attack = attack;
            IsHostile = isHostile;
            Inventory = new Inventory();
            if (items != null)
            {
                Inventory.Items.AddRange(items);
            }
        }
        public void TakeHit(int attack)
        {
            if (Defence < attack)
            {
                Health -= (attack - Defence);
            }
        }
    }
}
