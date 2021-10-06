using System;

namespace ELEKSUNI
{
    interface IEquipment
    {
        string Equip(Player player);
    }
    public class Item
    {
        public string Name { get; }
        public int Price { get; }
        public double Weight { get; set; }
        public Item(string name, int price, double weight)
        {
            this.Name = name;
            this.Price = price;
            this.Weight = weight;
        }
        public virtual string GetItemSpecs()
        {
            return $" { Name } { Weight } кг";
        }
        public virtual void PickThisItem(Player player)
        {
            player.inventory.AddItem(this);
        }
        public virtual void RemoveThisItem(Player player)
        {
            player.inventory.DropItem(this);
        }
        public virtual string UseThisItem(Player player)
        {
            if(this.Name == "Огниво")
            {
                return $" вы успешно разожгли костер и согрелись. Горячая пища вкуснее и лучше усваивается";
            }
            else
            {
                return $" не похоже, что это дало какой-либо эффект";
            }
        }
    }
}