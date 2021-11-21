using System;
using System.Collections.Generic;
using System.Text;

namespace ELEKSUNI
{
    class Inventory
    {
        public double Weight { get; private set; }
        public int Coins { get; private set; }
        public Item CurrentItem { get; set; }
        public List<Item> Items { get; private set; }
        public Inventory()
        {
            Weight = 0;
            Coins = 0;
            Items = new List<Item>();
        }
        public Inventory(List<Item> items, int coins = 0) : this()
        {
            this.Items.AddRange(items);
            Weight = CalculateWeight();
            Coins = coins;
        }
        public void Drop()
        {
            Items.Remove(CurrentItem);
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
            Items.Add(newItem);
            Weight += newItem.Weight;
        }
        public void Buy()
        {
            Coins -= CurrentItem.Price;
            Items.Add(CurrentItem);
            CurrentItem = null;
        }
        public void AddMoney(int amount)
        {
            Coins += amount;
        }
        private double CalculateWeight()
        {
            double weight = 0;
            foreach (var item in Items)
            {
                weight += item.Weight;
            }
            return weight;
        }
    }
}