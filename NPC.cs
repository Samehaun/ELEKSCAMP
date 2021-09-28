using System;
using System.Collections.Generic;
using System.Text;

namespace ELEKSUNI
{
    public class NPC : Entity
    {
        public bool IsHostile { get; set; }
        public NPC(string name, int health, int defence, int attack, Inventory inventory, bool isEnemy)
        {
            this.Name = name;
            this.Health = health;
            this.IsHostile = isEnemy;
            this.inventory = inventory;
            this.Attack = attack;
            this.Defence = defence;
            base.inventory = new Inventory();
        }
        public void Talk()
        {

        }
        public void Trade()
        {

        }
        public void StealFromNPC()
        {

        }
    }
}
