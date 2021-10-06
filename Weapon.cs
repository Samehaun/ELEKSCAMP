using System;


namespace ELEKSUNI
{
    public class Weapon : Item
    {
        public int Attack { get; set; }
        public Weapon(string name, int attack, int price, double weight ) : base (name, price, weight)
        {
            this.Attack = attack;
        }
        public override string GetItemSpecs()
        {
            return $"{ Name }  {Attack } атака {Weight } кг";
        }
        public override void PickThisItem(Player player)
        {
            player.inventory.AddItem(this);
            UseThisItem(player);
        }
        public override void UseThisItem(Player player)
        {
            player.Attack = this.Attack;
        }
        public override void RemoveThisItem(Player player)
        {
            if(player.Attack == this.Attack)
            {
                player.Attack = 0;
            }
            player.inventory.DropItem(this);
        }
    }
}
