using System;
using System.Collections.Generic;

namespace ELEKSUNI
{
    public class Item
    {
        private List<Keys> options;
        public Keys Name { get; }
        public int Price { get; }
        public double Weight { get; set; }
        protected string onUseMessage;
        public string Description { get; }
        public Item(Keys name, int price, double weight, string description, string useEffect)
        {
            this.Name = name;
            this.Price = price;
            this.Weight = weight;
            this.onUseMessage = useEffect;
            this.Description = description;
            options = new List<Keys>();
            options.Add(Keys.Use);
            options.Add(Keys.Drop);
            options.Add(Keys.Cancel);
        }
        public virtual string GetItemSpecs(string language)
        {
            return $" { Data.Localize(Name, language) } { Weight } {Data.Localize(Keys.Weight, language)}";
        }
        public virtual string UseThisItem()
        {
            return onUseMessage;
        }
        public virtual List<Keys> PossibleActions()
        {
            return this.options;
        }
    }
}