using System;
using System.Collections.Generic;
using System.Text;

namespace ELEKSUNI
{
    public class Effect
    {
        public string Name { get; }
        public double NegativeEffectMultiplier { get; set; }
        public bool IsActive { get; set; }
        public Effect(string name)
        {
            this.Name = name;
            this.IsActive = false;
        }
    }
}
