using System.Collections.Generic;

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
                inventory.items.AddRange(items);
            }
        }
        public void TakeHit(int attack)
        {
            if (Defence < attack)
            {
                Health -= (attack - Defence);
            }
        }
        public void Buy(Item item)
        {
            inventory.Add(item);
        }
        public void Drop(Item item)
        {
            inventory.Drop(item);
        }
        public Item GetItem(int index)
        {
            return inventory.GetItem(index);
        }
        public List<Item> GetListOfItemsInInventory()
        {
            return inventory.GetItemList();
        }
        public NPCSave Save()
        {
            return new NPCSave(this, inventory);
        }
        public void Load(NPCSave save, Prefabs prefabs)
        {
            Name = save.Name;
            Health = save.Health;
            Defence = save.Defence;
            Attack = save.Attack;
            IsHostile = save.IsHostile;
            inventory.Load(save.Inventory, prefabs);
        }
    }
    struct NPCSave
    {
        public Keys Name { get; set; }
        public int Health { get; set; }
        public int Defence { get; set; }
        public int Attack { get; set; }
        public InventorySave Inventory { get; set; }
        public bool IsHostile { get; set; }
        public NPCSave(NPC npc, Inventory inventory)
        {
            Name = npc.Name;
            Health = npc.Health;
            Defence = npc.Defence;
            Attack = npc.Attack;
            IsHostile = npc.IsHostile;
            Inventory = inventory.Save();
        }
    }
}
