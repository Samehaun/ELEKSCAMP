using System;
using System.Collections.Generic;
using System.Text;

namespace ProceduralQuest
{
    class Weapon : Item
    {
        public int Attack { get; private set; }
        public Weapon(Keys name, int price, double weight, Keys useMethod, int attack) : base (name, price, weight, useMethod)
        {
            Attack = attack;
        }
        public override string GetItemSpecs(string language)
        {
            return $" { Data.Localize(Name, language) } { Attack } { Data.Localize(Keys.Attack, language)} { Weight } { Data.Localize(Keys.Weight, language) }";
        }
        public override string GetItemSpecsForTrade(string language)
        {
            return $" { GetItemSpecs(language) } { Price } { Data.Localize(Keys.Coins, language) }";
        }
    }
}
