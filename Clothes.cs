using System;
using System.Collections.Generic;
using System.Text;

namespace ELEKSUNI
{
    class Clothes : Item
    {
        public int Warmth { get; set; }
        public int Defence { get; set; }
        public Clothes(string name, int warmth, int defence, int price, double weight) : base(name, price, weight)
        {
            this.Warmth = warmth;
            this.Defence = defence;
        }
    }
}
