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
        public List<String> GetPossibleInteractOptions()
        {
            List<string> posibilities = new List<string>();
            if(IsHostile)
            {
                posibilities.Add("Бежать");
                posibilities.Add("Драться");
            }
            else
            {
                posibilities.Add("Говорить");
                posibilities.Add("Торговать");
            }
            return posibilities;
        }
        public string Talk()
        {
            return $"вы не узнали ничего интересного";
        }
        public void Trade()
        {

        }
        public void StealFromNPC()
        {

        }
    }
}
