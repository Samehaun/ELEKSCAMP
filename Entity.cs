﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ELEKSUNI
{
     public abstract class Entity
    {
        public string Name { get; set; }
        public int Health { get; set; }
        public int Defence { get; set; }
        public int Attack { get; set; }
        public Inventory inventory;
    }
}