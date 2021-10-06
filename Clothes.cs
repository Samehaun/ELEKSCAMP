using System;


namespace ELEKSUNI
{
    public class Clothes : Item
    {
        public int Warmth { get; set; }
        public int Defence { get; set; }
        public Clothes(string name, int warmth, int defence, int price, double weight) : base(name, price, weight)
        {
            this.Warmth = warmth;
            this.Defence = defence;
        }
        public override string GetItemSpecs()
        {
            return $" { Name } защита { Weight } кг";
        }
        public override void PickThisItem(Player player)
        {
            player.inventory.AddItem(this);
        }
        public override void UseThisItem(Player player)
        {
            player.Warmth = this.Warmth;
            player.Defence = this.Defence;
        }
    }
}
