using System;


namespace ELEKSUNI
{
    public class Clothes : Item, IEquipment
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
            Equip(player);
        }
        public override string UseThisItem(Player player)
        {
            return Equip(player);
        }
        public string Equip(Player player)
        {
            if(player.Defence < this.Defence)
            {
                player.CurrentClothes = this;
                player.Warmth = this.Warmth;
                player.Defence = this.Defence;
                return $"вы экипировали { this.Name }";
            }
            else
            {
                return $" ваша текущая одежда имеет лучшие или аналогичные характеристики";
            }
        }
        public override void RemoveThisItem(Player player)
        {
            if (player.CurrentClothes == this)
            {
                player.Defence = 0;
            }
            player.inventory.DropItem(this);
            player.CurrentClothes = null;
        }
    }
}
