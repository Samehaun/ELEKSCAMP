using System;


namespace ELEKSUNI
{
    public class Weapon : Item
    {
        public int Attack { get; set; }
        public Weapon(Keys name, int attack, int price, double weight, string description, string useEffect) : base(name, price, weight, description, useEffect)
        {
            this.Attack = attack;
        }
        public override string GetItemSpecs(string language)
        {
            return $" { Data.Localize(Name, language) } {Attack} {Data.Localize(Keys.Attack, language)} { Weight } {Data.Localize(Keys.Weight, language)}";
        }
    }
}
