using System;
using System.Collections.Generic;
using System.Text;

namespace ELEKSUNI
{
    public class Consumable : Item
    {

        public Consumable(string name, int price, double weight) : base(name, price, weight)
        {

        }
        public override void UseThisItem(Player player)
        {

        }
    }
}
