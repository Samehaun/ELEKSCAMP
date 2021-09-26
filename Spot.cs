using System;
using System.Collections.Generic;
using System.Text;

namespace ELEKSUNI
{
    class Spot
    {
        public Index index;
        public string Cache { get; }
        public string Description { get; set; }
        private Item hidenObject;
        public Spot(Index coordinates, string hideout, string description, Item treasure)
        {
            this.index = coordinates;
            this.Cache = hideout;
            this.Description = description;
            this.hidenObject = treasure;
        }
    }
}
