using System;
using System.Collections.Generic;
using System.Text;

namespace ELEKSUNI
{
    class Weapon : Item
    {
        public int Attack { get; set; }
        public int Defence { get; set; }
        public Weapon(string name, int attack, int defence, int price, double weight ) : base (name, price, weight)
        {
            this.Attack = attack;
            this.Defence = defence;
        }
    }
}
