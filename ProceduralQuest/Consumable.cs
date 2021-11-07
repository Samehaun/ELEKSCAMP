using System;
using System.Collections.Generic;
using System.Text;

namespace ProceduralQuest
{
    class Consumable : Item
    {
        public int EffectPower { get; private set; }
        public bool AutoConsume { get; private set; }
        private Consumable(Keys name, int price, double weight, Keys useMethod, int effect, bool instantUse) : base(name, price, weight, useMethod)
        {
            EffectPower = effect;
            AutoConsume = instantUse;
        }
        public override string GetItemSpecs(string language)
        {
            return $" { Data.Localize(Name, language) } { Weight } { Data.Localize(Keys.Weight, language) }";
        }
        public override string GetItemSpecsForTrade(string language)
        {
            return $" { GetItemSpecs(language) } { Price } { Data.Localize(Keys.Coins, language) }";
        }
        public static Consumable CreatePoisoned(Keys name, int price, double weight, int effect)
        {
            return new Consumable(name, price, weight, Keys.Poisone, effect, false);
        }
        public static Consumable CreateFood(Keys name, int price, double weight, int effect)
        {
            return new Consumable(name, price, weight, Keys.Eat, effect, false);
        }
        public static Consumable CreateTrap(int effect)
        {
            return new Consumable(Keys.Trap, 0, 0, Keys.Trap, effect, true);
        }
        public static Consumable CreateHornetNest(int effect)
        {
            return new Consumable(Keys.HornetNest, 0, 0, Keys.HornetNest, effect, true);
        }
        public static Consumable CreateBandage(int price, double weight)
        {
            return new Consumable(Keys.Bondage, price, weight, Keys.StopBleeding, 0, false);
        }
        public static Consumable CreateCure(Keys name, int price, double weight, int effect)
        {
            return new Consumable(Keys.Cure, price, weight, Keys.CurePoison, effect, false);
        }
    }
}
