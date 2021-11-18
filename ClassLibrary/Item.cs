using System;
using System.Collections.Generic;

namespace ELEKSUNI
{
    class Item
    {
        public Keys Name { get; }
        public int Price { get; }
        public double Weight { get; }
        public List<Keys> Options { get; }
        public Item(Keys name, int price, double weight)
        {
            Name = name;
            Price = price;
            Weight = weight;
            Options = new List<Keys>() { Keys.Drop, Keys.Cancel };
        }
        public Item(Keys name, int price, double weight, Keys useMethod) : this (name, price, weight)
        {
            Options.Insert(0, useMethod);
        }
        public Keys Use()
        {
            return Options[0];
        }
        public virtual string GetItemSpecs(string language)
        {
            return $" { Data.Localize(Name, language) } { Weight } {Data.Localize(Keys.Weight, language)}";
        }
        public virtual string GetItemSpecsForTrade(string language)
        {
            return $" { GetItemSpecs(language) } { Price } {Data.Localize(Keys.Coins, language)}";
        }
    }
}