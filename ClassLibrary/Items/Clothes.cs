namespace ELEKSUNI
{
    class Clothes : Item
    {
        public int Defence { get; private set; }
        public int Warmth { get; private set; }
        public Clothes(Keys name, int price, double weight, Keys useMethod, int defence, int warmth) : base(name, price, weight, useMethod)
        {
            Defence = defence;
            Warmth = warmth;
        }
        public override string GetItemSpecs(string language)
        {
            return $"{ Data.Localize(Name, language) } { Defence } { Data.Localize(Keys.Defence, language) } { Weight } { Data.Localize(Keys.Weight, language) }";
        }
        public override string GetItemSpecsForTrade(string language)
        {
            return $"{ GetItemSpecs(language) } { Price } { Data.Localize(Keys.Coins, language) }";
        }
    }
}