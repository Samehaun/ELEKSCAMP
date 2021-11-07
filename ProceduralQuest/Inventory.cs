using System;
using System.Collections.Generic;
using System.Text;

namespace ProceduralQuest
{
    class Inventory
    {
        public double Weight { get; private set; }
        public int Coins { get; private set; }
        public Item CurrentItem { get; set; }
        private List<Item> items;
        public Inventory()
        {
            Weight = 0;
            Coins = 0;
            items = new List<Item>();
        }
        public Inventory(List<Item> items) : this()
        {
            this.items = items;
        }
        public void Drop()
        {
            items.Remove(CurrentItem);
            Weight -= CurrentItem.Weight;
            CurrentItem = null;
        }
        public void Sell()
        {
            Coins += CurrentItem.Price;
            Drop();
        }
        public void Add(Item newItem)
        {
            items.Add(newItem);
            Weight += newItem.Weight;
        }
        public void Buy()
        {
            Coins -= CurrentItem.Price;
            items.Add(CurrentItem);
        }
    }
}
