using System;
namespace ELEKSUNI
{
    public class Clothes : Item
    {
        public int Warmth { get; set; }
        public int Defence { get; set; }
        public Clothes(Keys name, int warmth, int defence, int price, double weight, string description, string useEffect) : base(name, price, weight, description, useEffect)
        {
            this.Warmth = warmth;
            this.Defence = defence;
        }
        public override string GetItemSpecs(string language)
        {
            return $" { Data.Localize(Name, language) } {Defence} {Data.Localize(Keys.Defence, language)} { Weight } {Data.Localize(Keys.Weight, language)}";
        }
    }
}