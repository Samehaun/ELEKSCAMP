using System;
using System.Collections.Generic;
using System.Text;

namespace ELEKSUNI
{
    class Player : Entity
    {
        public int Coins { get; set; }
        public List<Effect> effects;
        public Effect isHungry, isBleeding, isFrozen, isPoisoned, isInjured;
        public Spot Position { get; set; }
        public Player(string playerName)
        {
            this.Name = playerName;
            this.Coins = 0;
            this.Health = 100;
            this.Defence = 0;
            isBleeding = new Effect("кровотечение");
            isFrozen = new Effect("обморожение");
            isHungry = new Effect("голод");
            isInjured = new Effect("травма");
            isPoisoned = new Effect("отравление");
            effects = new List<Effect>() { isBleeding, isFrozen, isHungry, isInjured, isPoisoned };
        }
        public void CurrentState()
        {
            StringBuilder activeEffects = new StringBuilder();
            foreach (var effect in effects)
            {
                if (effect.IsActive)
                {
                    activeEffects.Append(" " + effect.Name + " ");
                }
            }
            Console.WriteLine($" У игрока { Name } { Coins } монет { Health } хп { activeEffects.ToString() }");
        }
    }
}
