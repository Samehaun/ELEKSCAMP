using System;
using System.Collections.Generic;

namespace ELEKSUNI
{
    public class Player : Entity
    {
        private double staminaRegenRate;
        private double stamina;
        private double speed;
        public int Coins { get; set; }
        public Player(string playerName)
        {
            this.staminaRegenRate = 3;
            this.Name = playerName;
            this.Coins = 0;
            this.Health = 100;
            this.Defence = 0;
            this.speed = (int)MainQuestConfig.BasePlayerSpeed;
            this.stamina = 100;
            this.Attack = 0;
        }
        public string GetCurrentState()
        {
            return $" У игрока { Name } { Coins } монет { Health } хп ";
        }
        public double Rest()
        {
            if(stamina < 100)
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
    }
}
