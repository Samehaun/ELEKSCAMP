using System;
using System.Collections.Generic;

namespace ELEKSUNI
{
    public class Player : Entity
    {
        public Spot currentSpot;
        private double stamina;
        private double speed;
        public Weapon CurrentWeapon { get; set; }
        public Clothes CurrentClothes { get; set; }
        public int Coins { get; set; }
        public int Warmth { get; set; }
        public Spot CurrentPosition { get; set; }
        public Player(string playerName)
        {
            this.Name = playerName;
            this.Coins = 0;
            this.Health = 100;
            this.Defence = 0;
            this.Warmth = 5;
            this.speed = (int)MainQuestConfig.BasePlayerSpeed;
            this.stamina = 100;
            this.Attack = 0;
            base.inventory = new Inventory();
        }
        public string GetCurrentState()
        {
            return $" У игрока { Name } { Coins } монет { Health } хп ";
        }
        public List<string> GetListOfPossibleOptions()
        {
            List<string> posibilities = new List<string>();
            posibilities.Add("Идти");
            posibilities.Add("Отдыхать");
            posibilities.Add("Спать");
            posibilities.Add("Искать");
            posibilities.Add("Инвентарь");
            if(currentSpot.NPC != null)
            {
                posibilities.Add(currentSpot.NPC.Name);
            }
            return posibilities;
        }
        public void DesideWhatToDo(int chosenOption)
        {
           
        }
        
        public void Rest()
        {

        }
        public void Sleep()
        {

        }
        public void Fight(NPC enemy)
        {

        }
        public void Retreat(NPC enemy)
        {

        }
        public double OverweightFactor()
        {
            return 1 + Math.Sqrt(inventory.GetTotalWeight() / (int)MainQuestConfig.MaxWeigtPlayerCanCarry);
        }
        public double GetPlayerSpeed()
        {
            return speed;
        }
        public double GetPlayerStamina()
        {
            return stamina;
        }
    }
}
