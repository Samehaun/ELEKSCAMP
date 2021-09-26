using System;
using System.Collections.Generic;
using System.Text;

namespace ELEKSUNI
{
    class Effect
    {
        public string Name { get; }
        public bool IsActive { get; set; }
        public Effect(string name)
        {
            this.Name = name;
            this.IsActive = false;
        }
    }
}
