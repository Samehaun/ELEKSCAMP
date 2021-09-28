using System;
using System.Collections.Generic;
using System.Text;

namespace ELEKSUNI
{
    public class Inventory
    {
        private List<Item> items;
        public Inventory()
        {
            items = new List<Item>();
        }
        public void ShowInventory()
        {
            Console.Clear();
            foreach (var item in items)
            {
                item.PrintItemSpecs();
            }
        }
        public void AddItem(Item newItem)
        {
            items.Add(newItem);
        }
        public void DropItem(int index)
        {
            items.RemoveAt(index);
        }
        public double GetTotalWeight()
        {
            double totalWeight = 0;
            foreach (var item in items)
            {
                totalWeight += item.Weight;
            }
            return totalWeight;
        }
        public Item GetItem(int index)
        {
            return items[index];

        }
    }
}
