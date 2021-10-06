using System;
using System.Collections.Generic;
using System.Text;

namespace ELEKSUNI
{
    public class Inventory
    {
        private List<Item> items;
        private double totalWeight;
        public Inventory()
        {
            items = new List<Item>();
            totalWeight = 0;
        }
        public List<string> ShowInventory()
        {
            List<string> inventory = new List<string>();
            foreach (var item in items)
            {
                inventory.Add(item.GetItemSpecs());
            }
            return inventory;
        }
        public void AddItem(Item newItem)
        {
            items.Add(newItem);
            totalWeight += newItem.Weight;
        }
        public void DropItem(Item item)
        {
            totalWeight -= item.Weight;
            items.Remove(item);
        }
        public double GetTotalWeight()
        {
            return totalWeight;
        }
        //private void EquipTheTheBest()
        //{
        //    foreach (var item in items)
        //    {
        //        if(item is IEquipment)
        //        {
        //            (IEquipment)item.
        //        }
        //    }
        //}
    }
}
