﻿using System;
using System.Collections.Generic;

namespace ProceduralQuest
{
    public class Item
    {
        public Keys Name { get; }
        public int Price { get; }
        public double Weight { get; }
        public Keys Usage { get;  }
        public Item(Keys name, int price, double weight, Keys useMethod)
        {
            Name = name;
            Price = price;
            Weight = weight;
            Usage = useMethod;
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