﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ELEKSUNI
{
     abstract class Entity
    {
        public string Name { get; set; }
        public int Health { get; set; }
        public int Defence { get; set; }
        public Inventory inventory;
    }
}
