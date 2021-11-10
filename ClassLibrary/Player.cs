using System;
using System.Collections.Generic;

namespace ELEKSUNI
{
    class Player
    {
        private double staminaRegenRate;
        private double stamina;
        private double hunger, heat;
        public string Name { get; private set; }
        public int Health { get; private set; }
        public Weapon CurrentWeapon { get; set; }
        public Clothes CurrentClothes { get; set; }
        public List<Keys> effects { get; private set; }
        public Inventory inventory;
        private double hungerModifier;
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
            double time = CalculateStaminaNeededToTravel() / staminaRegenRate;
            if (stamina < 100)
            {
                stamina += CalculateStaminaNeededToTravel();
            }
            hungerModifier = 1.3;
            innerStateProcess(time);
            return time;
        }
        public double Sleep()
        {
            stamina = 100;
            double time = 100 / (staminaRegenRate * 3);
            hungerModifier = 1;
            innerStateProcess(time);
            return time;
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
            hungerModifier = 2;
            innerStateProcess(CalculateTimeNeededToTravel());
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
        private void innerStateProcess(double time)
        {
            hunger += time * hungerModifier;
            Health -= (int)((hunger / 100) * time);
        }
    }
}