using System;
using System.Collections.Generic;
using System.Text;

namespace ELEKSUNI
{
    class MainQuest
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter your name");
            Player player = new Player(Console.ReadLine());
            ////Console.WriteLine(player.name);
            ////Console.WriteLine(player.health);
            ////Console.WriteLine(player.coin);
            player.CurrentState();
            //Console.WriteLine("Вы очнулись в лесу ваши действия:");
            //Console.WriteLine("1: Позвать на помощь");
            //Console.WriteLine("2: Осмотреться");
            //Console.WriteLine("3: Ждать");
            //switch (Convert.ToInt32(Console.ReadLine()))
            //{
            //    case 1:
            //        Console.WriteLine("Упс... Вы привлекли внимание медведя");
            //        Console.WriteLine("Бежать");
            //        Console.WriteLine("Драться");
            //        Console.WriteLine("Попытаться напугать");
            //        break;
            //    case 2:
            //        Console.WriteLine("В груде листьев вы нашли нож.");
            //        Console.WriteLine("");

            //        break;
            //    case 3:
            //        Console.WriteLine("Вы замерзли и проголодались");
            //        player.health = player.health - 10;
            //        break;
            //    default:
            //        Console.WriteLine("Неверный ввод");
            //        break;
            //}
  
        }
    }
}
