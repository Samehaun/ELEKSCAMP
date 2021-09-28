using System;


namespace ELEKSUNI
{
    public class Weapon : Item
    {
        public int Attack { get; set; }
        public int Defence { get; set; }
        public Weapon(string name, int attack, int defence, int price, double weight ) : base (name, price, weight)
        {
            this.Attack = attack;
            this.Defence = defence;
        }
        public override void PrintItemSpecs()
        {
            Console.WriteLine($"{ Name }  {Attack } атака { Defence } защита {Weight } кг");
        }
        public override void PickAnItem(Player player)
        {
            player.inventory.AddItem(this);
            UseThisItem(player);
        }
        public override void UseThisItem(Player player)
        {
            player.Attack = this.Attack;
            player.Defence += this.Defence;
        }
    }
}
