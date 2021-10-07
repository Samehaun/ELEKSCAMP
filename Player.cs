using System;
using System.Collections.Generic;

namespace ELEKSUNI
{
    public class Player : Entity
    {
        private double stamina;
        private double speed;
        public int Coins { get; set; }
        public Player(string playerName)
        {
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
        public string Rest()
        {
                return "Вы немного отдохнули";
        }
        public string Sleep()
        {
                return "Вы полны сил";

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
