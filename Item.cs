using System;

namespace ELEKSUNI
{
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
        public virtual void UseThisItem(Player player)
        {

        }
    }
}