using System;


namespace ELEKSUNI
{
    public class Weapon : Item, IEquipment
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
            Equip(player);
        }
        public override string UseThisItem(Player player)
        {
            return Equip(player);
        }
        public override void RemoveThisItem(Player player)
        {
            if(player.CurrentWeapon == this)
            {
                player.Attack = 0;
            }
            player.inventory.DropItem(this);
            player.CurrentWeapon = null;
        }
        public string Equip(Player player)
        {
            if(player.Attack < this.Attack)
            {
                player.CurrentWeapon = this;
                player.Attack = this.Attack;
                return $" Вы экипировали { this.Name }";
            }
            else
            {
                return $" Ваше текущее оружие имеет лучшие или аналогичные характеристики";
            }

        }

    }
}
