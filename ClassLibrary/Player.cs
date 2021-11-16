using System;
using System.Collections.Generic;

namespace ELEKSUNI
{
    class Player
    {
        private double staminaRegenRate;
        private double stamina;
        private double hunger;
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
            InnerStateProcess(time);
            return time;
        }
        public double Sleep()
        {
            stamina = 100;
            double time = 100 / (staminaRegenRate * 3);
            hungerModifier = 1;
            InnerStateProcess(time);
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
        public void RecaculateStateDueToTraveling(int additionalStaminaConsumptionModifier = 1)
        {
            stamina -= CalculateStaminaNeededToTravel() * additionalStaminaConsumptionModifier;
            hungerModifier = 2;
            InnerStateProcess(CalculateTimeNeededToTravel() * additionalStaminaConsumptionModifier);
        }
        private double Speed()
        {
            double speed = (int)MainQuestConfig.BasePlayerSpeed;
            double maxWeight = (int)MainQuestConfig.MaxWeightPlayerCanCarry;
            if (maxWeight < inventory.Weight)
            {
                speed *= Math.Sqrt(maxWeight / inventory.Weight);
            }
            return speed;
        }
        public void InnerStateProcess(double time)
        {
            hunger += time * hungerModifier;
            Health -= (int)((hunger / 100) * time);
        }
        public void TakeHit(int attack)
        {
            if(CurrentClothes == null)
            {
                Health -= attack;
            }
            else if(CurrentClothes.Defence < attack)
            {
                Health -= (attack - CurrentClothes.Defence);
            }
        }
    }
}