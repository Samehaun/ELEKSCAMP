using System;
using System.Collections.Generic;
using System.Text;

namespace ELEKSUNI
{
    class MainQuest
    {
        private string name;
        static void Main(string[] args)
        {
            Console.WriteLine("Enter your name");
            Player player = new Player(Console.ReadLine());
            Console.WriteLine(player.name);
            Console.WriteLine(player.health);
            Console.WriteLine(player.coin);
        }
    }
}
