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
        BaseTimeToChangeLocation = 3,
        MapSize = 4
    }
    class MainQuest
    {
        static void Main(string[] args)
        {
            Map questMap = CreatePredefinedMap();
            Player player = CreatePlayer();
            player.currentSpot = questMap.GetSpotByCoordinate((1, 1));
            int playerInput;
            Console.WriteLine(player.GetCurrentState());
            ShowAvailableOptions(player.GetListOfPossibleOptions());
        }
        public static Player CreatePlayer()
        {
            Console.WriteLine("Enter your name");
            Player player = new Player(Console.ReadLine());
            player.inventory.AddItem(new Clothes("простая одежда", 0, 0, 5, 1.0));
            return player;
        }
        public static void ShowAvailableOptions(List<string> posibilities)
        {
            int i = 0;
            foreach (var option in posibilities)
            {
                Console.WriteLine($" { i++ } - { option }");
            }
            Console.WriteLine($" { i } - назад");
        }
        public static bool CheckInput(out int input, List<string> posibiliies)
        {
            input = Convert.ToInt32(Console.ReadLine());
            if (input > 0 && input < posibiliies.Count)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
       
        public static Map CreatePredefinedMap()
        {
            Map questMap = new Map();
            questMap.AddSpot(new Spot((0, 0), "Лес как и везде. Не видно ничего необычного кроме опавшей листвы", new Item("Огниво", 25, 0.3), null));
            questMap.AddSpot(new Spot((0, 1), "Лес как и везде. Не видно ничего необычного кроме опавшей листвы", null, null));
            questMap.AddSpot(new Spot((0, 2), "Лес как и везде. Не видно ничего необычного кроме опавшей листвы", null, null));
            questMap.AddSpot(new Spot((1, 0), "Много кустарника. Возможно, возможно ягоды съедобны", new Weapon("Нож", 15, 25, 0.3), null));
            questMap.AddSpot(new Spot((1, 1), "Лес как и везде. Не видно ничего необычного кроме опавшей листвы", new Item("Огниво", 25, 0.3), null));
            questMap.AddSpot(new Spot((1, 2), "Много кустарника. Возможно, возможно ягоды съедобны", new Item("Огниво", 25, 0.3), null));
            questMap.AddSpot(new Spot((2, 0), "Лес как и везде. Не видно ничего необычного кроме опавшей листвы", null, null));
            questMap.AddSpot(new Spot((2, 1), "Много кустарника. Возможно, возможно ягоды съедобны", new Item("Огниво", 25, 0.3), null));
            questMap.AddSpot(new Spot((2, 2), "Лес как и везде. Не видно ничего необычного кроме опавшей листвы", new Item("Огниво", 25, 0.3), null));
            return questMap;
        }
        //public static Map CreateNewMap(int xDimension, int yDimension)
        //{
        //    Map questMap = new Map();
        //    for (int i = 0; i < xDimension; i++)
        //    {
        //        for (int j = 0; j < yDimension; j++)
        //        {
        //            questMap.AddSpot(new Spot((i, j), GetDescription(spotDesccriptionDatabase()), ChoseRandomItem(ItemDatabse())));
        //        }
        //    }

        //    return questMap;
        //}
        //static string GetDescription(List<String> descriptions)
        //{
        //    //add some additional constrains later on

        //    Random rnd = new Random(DateTime.Now.Minute);
        //    return descriptions[rnd.Next(0, descriptions.Count - 1)];
        //}
        //static List<string> spotDesccriptionDatabase()
        //{
        //    List<string> descriptions = new List<string>();

        //    //add text getter from some file or DB

        //    return descriptions;
        //}
        //static Item ChoseRandomItem(List<Item> itemDatabase)
        //{
        //    //add some additional constrains later on

        //    Random rnd = new Random(DateTime.Now.Second);
        //    return itemDatabase[rnd.Next(0, itemDatabase.Count - 1)];
        //}
        //static List<Item> ItemDatabse()
        //{
        //    List<Item> items = new List<Item>();

        //    //add text getter from some file or DB

        //    return items;
        //}
    }
}
