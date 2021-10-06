using System;
using System.Collections.Generic;
using System.Text;

namespace ELEKSUNI
{
    public class Player : Entity
    {
        private Spot currentSpot;
        private double stamina;
        private double speed;
        private Weapon currentWeapn;
        private Clothes currentClothes;
        public int Coins { get; set; }
        public int Warmth { get; set; }
        //private List<Effect> negativeEffects;
        //private Effect isHungry, isBleeding, isFrozen, isPoisoned, isInjured;
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
            //isBleeding = new Effect("кровотечение");
            //isFrozen = new Effect("обморожение");
            //isHungry = new Effect("голод");
            //isInjured = new Effect("травма");
            //isPoisoned = new Effect("отравление");
            //negativeEffects = new List<Effect>() { isBleeding, isFrozen, isHungry, isInjured, isPoisoned };
            this.Attack = 0;
            base.inventory = new Inventory();
        }
        public string GetCurrentState()
        {
            //StringBuilder activeEffects = new StringBuilder();
            //foreach (var effect in negativeEffects)
            //{
            //    if (effect.IsActive)
            //    {
            //        activeEffects.Append(" " + effect.Name + " ");
            //    }
            //}
            //return $" У игрока { Name } { Coins } монет { Health } хп { activeEffects.ToString() }";
            return $" У игрока { Name } { Coins } монет { Health } хп ";
        }
        public void ShowAvailableOptions()
        {

        }
        public void Travel()
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
        private double CalculateTimeNeededToTravel()
        {
            double time = (int)MainQuestConfig.BaseTimeToChangeLocation * ((int)MainQuestConfig.BasePlayerSpeed / speed);
            return time;
        }
        private double CalculateStaminaNeededToTravel()
        {
            return CalculateTimeNeededToTravel() * (int)MainQuestConfig.BasePlayerStaminaConsuption * NegativeEffectsMultiplier();
        }
        private double NegativeEffectsMultiplier()
        {
            double multiplier = 1;
            foreach (var effect in negativeEffects)
            {
                if (effect.IsActive)
                    multiplier *= effect.NegativeEffectMultiplier;
            }
            return multiplier;
        }
    }
}
