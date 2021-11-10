using System;
using System.Collections.Generic;

namespace ELEKSUNI
{
    class Player : Entity
    {
        private double staminaRegenRate;
        private double stamina;
        private double hunger, heat;
        public Weapon CurrentWeapon { get; set; }
        public Clothes CurrentClothes { get; set; }
        public List<Keys> effects { get; private set; }
        public Inventory inventory;
        public Player(string playerName)
        {
            staminaRegenRate = 3;
            Name = playerName;
            Health = 100;
            stamina = 100;
            heat = 100;
            hunger = 0;
            inventory = new Inventory();
        }
        public double Rest()
        {
            if (stamina < 100)
            {
                stamina += CalculateStaminaNeededToTravel();
            }
            return CalculateStaminaNeededToTravel() / staminaRegenRate;
        }
        public double Sleep()
        {
            stamina = 100;
            return 100 / (staminaRegenRate * 3);
        }
        public double GetPlayerStamina()
        {
            return stamina;
        }
        public double CalculateTimeNeededToTravel()
        {
            double time = (int)MainQuestConfig.BaseTimeToChangeLocation * ((int)MainQuestConfig.BasePlayerSpeed / Speed());
            return time;
        }
        public double CalculateStaminaNeededToTravel()
        {
            return CalculateTimeNeededToTravel() * (int)MainQuestConfig.BasePlayerStaminaConsuption;
        }
        public void RecaculateStateDueToTraveling()
        {
            stamina -= CalculateStaminaNeededToTravel();
        }
        private double Speed()
        {
            double speed = (int)MainQuestConfig.BasePlayerSpeed;
            double maxWeight = (int)MainQuestConfig.MaxWeigtPlayerCanCarry;
            if (maxWeight < inventory.Weight)
            {
                speed *= Math.Sqrt(maxWeight / inventory.Weight);
            }
            return speed;
        }
        private void innerStateProcess()
        {

        }
    }
}