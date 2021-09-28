using System;
using System.Collections.Generic;
using System.Text;

namespace ELEKSUNI
{
    enum MainQuestConfig 
    {
        BasePlayerSpeed = 5,
        BasePlayerStaminaConsuption = 1,
        MaxWeigtPlayerCanCarry = 10,
        BaseTimeToChangeLocation = 3
    }
    class MainQuest
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Enter your name");
            //Player player = new Player(Console.ReadLine());
            //player.PrintCurrentState();
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
        static Map CreateNewMap(int xDimension, int yDimension)
        {
            Map questMap = new Map();
            for (int i = 0; i < xDimension; i++)
            {
                for (int j = 0; j < yDimension; j++)
                {
                    questMap.AddSpot(new Spot((i, j), GetDescription(spotDesccriptionDatabase()), ChoseRandomItem(ItemDatabse())));
                }
            }

            return questMap;
        }
        static string GetDescription(List<String> descriptions)
        {
            //add some additional constrains later on

            Random rnd = new Random(DateTime.Now.Minute);
            return descriptions[rnd.Next(0, descriptions.Count - 1)];
        }
        static List<string> spotDesccriptionDatabase()
        {
            List<string> descriptions = new List<string>();

            //add text getter from some file or DB

            return descriptions;
        }
        static Item ChoseRandomItem(List<Item> itemDatabase)
        {
            //add some additional constrains later on

            Random rnd = new Random(DateTime.Now.Second);
            return itemDatabase[rnd.Next(0, itemDatabase.Count - 1)];
        }
        static List<Item> ItemDatabse()
        {
            List<Item> items = new List<Item>();

            //add text getter from some file or DB

            return items;
        }
    }
}
