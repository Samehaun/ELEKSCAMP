using System;
using System.Collections.Generic;
using System.Linq;

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
        public Weapon SelectBestWeapon()
        {
            List<Weapon> weapons = (List<Weapon>)from item in items
                                                 where item is Weapon
                                                 select item;
            if (weapons?.Count > 1)
            {
                return weapons.Aggregate((i1, i2) => i1.Attack > i2.Attack ? i1 : i2);
            }
            else if (weapons?.Count == 1)
            {
                return weapons[0];
            }
            else return null;
        }
        public Clothes SelectBestClothes()
        {
            List<Clothes> clothes = (List<Clothes>)from item in items
                                                   where item is Clothes
                                                   select item;
            if (clothes?.Count > 1)
            {
                return clothes.Aggregate((i1, i2) => i1.Defence > i2.Defence ? i1 : i2);
            }
            else if (clothes?.Count == 1)
            {
                return clothes[0];
            }
            else return null;
        }
        public Item GetItemByNumber(int index)
        {
            return items[index];
        }
        public List<string> Show(string language)
        {
            List<string> itemsSpecs = new List<string>();
            foreach (var item in items)
            {
                itemsSpecs.Add(item.GetItemSpecs(language));
            }
            itemsSpecs.Add(Data.Localize(Keys.Cancel, language));
            return itemsSpecs;
        }
    }
}
