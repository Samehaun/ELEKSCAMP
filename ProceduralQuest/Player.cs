using System;
using System.Collections.Generic;
using System.Text;

namespace ProceduralQuest
{
    class Player
    {
        private double stamina;
        private double speed;
        private double hpRegenRate, speedModifier, staminaConsumptionModifier, staminaRegenModifier, hpRegenModifier, staminaRegenRate;
        private bool isPoisoned, isBleeding, isInjured, isHunger;
        public string Name { get; }
        public Weapon CurrenWeapon { get; private set; }
        public Clothes CurrentClothes { get; private set; }
        public int HP { get; set; }
        public Player(string playerName, Clothes clothes)
        {
            hpRegenRate = (int)MainQuestConfig.HPBaseRegen;
            staminaRegenRate = (int)MainQuestConfig.BaseStaminaRegenRate;
            Name = playerName;
            HP = 100;
            speed = (int)MainQuestConfig.BasePlayerSpeed;
            stamina = 100;
            Equip(clothes);
            speedModifier = 1;
            staminaConsumptionModifier = 1;
            staminaRegenModifier = 1;
            hpRegenModifier = 1;
            isPoisoned = false;
            isHunger = false;
            isBleeding = false;
            isInjured = false;

        }
        public double Rest()
        {
            double time = CalculateStaminaNeededToTravel() / staminaRegenRate;
            if (stamina < 100)
            {
                stamina += CalculateStaminaNeededToTravel();
            }

            return time;
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
            double time = (int)MainQuestConfig.BaseTimeToChangeLocation * ((int)MainQuestConfig.BasePlayerSpeed / speed);
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
        public void Equip(Clothes newClothes)
        {
            CurrentClothes = newClothes;
        }
        public void Equip(Weapon newWeapon)
        {
            CurrenWeapon = newWeapon;
        }
        public void ApplyPoison()
        {
            isPoisoned = true;
            hpRegenModifier += Data.PoisonHPRegenModifier;
            staminaConsumptionModifier *= Data.PoisonStaminaConsumtionModifier;
            staminaRegenModifier *= Data.PoisonStaminaRegenMidifier;
        }
        public void CurePoison()
        {
            isPoisoned = false;
            hpRegenModifier -= Data.PoisonHPRegenModifier;
            staminaConsumptionModifier /= Data.PoisonStaminaConsumtionModifier;
            staminaRegenModifier /= Data.PoisonStaminaRegenMidifier;
        }
        public void ApplyBleeding()
        {
            isBleeding = true;
            hpRegenModifier += Data.BleedingHPRegenModifier;
            staminaConsumptionModifier *= Data.BleedingStaminaConsumtionModifier;
            staminaRegenModifier *= Data.BleedingStaminaRegenMidifier;
        }
        public void StopBleeding()
        {
            isBleeding = false;
            hpRegenModifier -= Data.BleedingHPRegenModifier;
            staminaConsumptionModifier /= Data.BleedingStaminaConsumtionModifier;
            staminaRegenModifier /= Data.BleedingStaminaRegenMidifier;
        }
        public void ApplyInjury()
        {
            isInjured = true;
            hpRegenModifier += Data.BleedingHPRegenModifier;
            staminaConsumptionModifier *= Data.BleedingStaminaConsumtionModifier;
            staminaRegenModifier *= Data.BleedingStaminaRegenMidifier;
        }
    }
}
