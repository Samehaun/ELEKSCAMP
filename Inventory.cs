using System;
using System.Collections.Generic;
using System.Text;

namespace ELEKSUNI
{
    class Inventory
    {
        private List<Item> items;
        public Inventory()
        {
            items = new List<Item>();
        }
        public void Show()
        {
            Console.Clear();
            int orderNumber = 0;
            foreach (var item in items)
            {
                if(item.GetType().Equals(typeof(Weapon)))
                {
                    Weapon weapon = (Weapon)item;
                    Console.WriteLine($" { orderNumber++ }  { weapon.Name }  { weapon.Attack } атака { weapon.Defence } защита { weapon.Weight } кг");
                }
                else if(item.GetType().Equals(typeof(Clothes)))
                {
                    Clothes armor = (Clothes)item;
                    Console.WriteLine($" { orderNumber++ }  { armor.Name } { armor.Defence } защита { armor.Weight } кг");
                }
                else
                {
                    Console.WriteLine($" { orderNumber++ } { item.Name } { item.Weight } кг");
                }
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
    }
}
