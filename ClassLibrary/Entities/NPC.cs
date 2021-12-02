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
        public Inventory inventory;
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
        public NPC()
        {
            inventory = new Inventory();
        }
        public NPC(Keys name, int health, int defence, int attack, bool isHostile, List<Item> items = null)
        {
            Name = name;
            Health = health;
            Defence = defence;
            Attack = attack;
            IsHostile = isHostile;
            inventory = new Inventory();
            if (items != null)
            {
                inventory.Items.AddRange(items);
            }
        }
        public void TakeHit(int attack)
        {
            if (Defence < attack)
            {
                Health -= (attack - Defence);
            }
        }
        public NPCSave Save()
        {
            return new NPCSave(this);
        }
        public void Load(NPCSave save, Prefabs prefabs)
        {
            Name = save.Name;
            Health = save.Health;
            Defence = save.Defence;
            Attack = save.Attack;
            IsHostile = save.IsHostile;
            foreach (var key in save.Inventory)
            {
                inventory.Add(prefabs.GetItemByKey(key));
            }
        }
    }
    struct NPCSave
    {
        public Keys Name { get; set; }
        public int Health { get; set; }
        public int Defence { get; set; }
        public int Attack { get; set; }
        public List<Keys> Inventory { get; set; }
        public bool IsHostile { get; set; }
        public NPCSave(NPC npc)
        {
            Name = npc.Name;
            Health = npc.Health;
            Defence = npc.Defence;
            Attack = npc.Attack;
            IsHostile = npc.IsHostile;
            Inventory = new List<Keys>();
            foreach (var item in npc.inventory.Items)
            {
                Inventory.Add(item.Name);
            }
        }
    }
}
