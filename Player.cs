using System;
using System.Collections.Generic;
using System.Text;

namespace ELEKSUNI
{
    class Player
    {
        public string name { get; set; }
        public int coin { get; set; }
        public int health { get; set; }
        public Player(string playerName)
        {
            name = playerName;
            coin = 0;
            health = 100;
        }
        public void CurrentState()
        {
            Console.WriteLine($"{ name } has { coin  } coins and { health } health");
        }
    }
}
