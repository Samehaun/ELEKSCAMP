using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace ELEKSUNI
{
    class Inventory
    {
        public double Weight { get; private set; }
        public int Coins { get; private set; }
        public Item CurrentItem { get; set; }
        public List<Item> items;
        public Inventory()
        {
            Weight = 0;
            Coins = 0;
            items = new List<Item>();
        }
        public Inventory(List<Item> items, int coins = 0) : this()
        {
            this.items.AddRange(items);
            Weight = CalculateWeight();
            Coins = coins;
        }
        public void DropSelected()
        {
            Drop(CurrentItem);
            CurrentItem = null;
        }
        public void Drop(Item item)
        {
            items.Remove(item);
            Weight -= item.Weight;
        }
        public Item GetItem(int index)
        {
            return items[index];
        }
        public List<Item> GetItemList()
        {
            return items;
        }
        public void Add(Item newItem)
        {
            items.Add(newItem);
            Weight += newItem.Weight;
        }
        public List<Keys> GetItemsNameList()
        {
            return (from item in items select item.Name).ToList<Keys>();
        }
        public int GetItemsCount()
        {
            return items.Count();
        }
        public void AddMoney(int amount)
        {
            Coins += amount;
        }
        private double CalculateWeight()
        {
            double weight = 0;
            foreach (var item in items)
            {
                weight += item.Weight;
            }
            return weight;
        }
        public InventorySave Save()
        {
            return new InventorySave(this);
        }
        public void Load(InventorySave save, Prefabs prefabs)
        {
            Weight = save.Weight;
            Coins = save.Coins;
            if(save.CurrentItem != null)
            {
                CurrentItem = prefabs.GetItemByKey((Keys)save.CurrentItem);
            }
            foreach (var key in save.Items)
            {
                items.Add(prefabs.GetItemByKey(key));
            }
        }
    }
    struct InventorySave
    {
        public double Weight { get; set; }
        public int Coins { get; set; }
        public Keys? CurrentItem { get; set; }
        public List<Keys> Items { get; set; }
        public InventorySave(Inventory inventory)
        {
            Weight = inventory.Weight;
            Coins = inventory.Coins;
            if(inventory.CurrentItem != null)
            {
                CurrentItem = inventory.CurrentItem.Name;
            }
            else
            {
                CurrentItem = null;
            }
            Items = inventory.GetItemsNameList();
        }
    }
}