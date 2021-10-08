using System;
using System.Collections.Generic;
using System.Text;

namespace ELEKSUNI
{
    enum MainQuestConfig 
    {
        BasePlayerSpeed = 5,
        BasePlayerStaminaConsuption = 5,
        MaxWeigtPlayerCanCarry = 10,
        BaseTimeToChangeLocation = 3,
        MapSize = 4
    }
    class MainQuest
    {
        Player player;
        Map questMap;
        bool gameOver;
        public Random randomizer;
        public MainQuest()
        {
            randomizer = new Random(DateTime.Now.Minute);
            gameOver = false;
            player = CreatePlayer();
            questMap = CreateNewMap((int)MainQuestConfig.MapSize, player);
            questMap.SetPlayerLocation(((int)MainQuestConfig.MapSize / 2, (int)MainQuestConfig.MapSize / 2));
            ClearOutdateInfo();
            Output("Вы пришли в себя в незнакомом месте. Неизвестно как вы здесь оказались, но по крайней мере вы живы и здоровы... пока");
        }
        static void Main(string[] args)
        {
            MainQuest quest = new MainQuest();
            quest.Play();           
        }
        public Player CreatePlayer()
        {
            Output("Enter your name");
            Player player = new Player(Console.ReadLine());
            return player;
        }
        public static void ShowAvailableOptions(List<string> posibilities)
        {
            int i = 0;
            foreach (var option in posibilities)
            {
               Output($" { i++ } - { option }");
            }
        }
        public static bool CheckInput(string input, List<string> posibilities)
        {
            int inputNumber;
            try
            {
                inputNumber = Convert.ToInt32(input);
            }
            catch (Exception)
            {
                return false;
            }
            if (inputNumber >= 0  && inputNumber < posibilities.Count)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static int GetConsoleInput(List<string> posibilities)
        {
            string input;
            do
            {
                input = Console.ReadLine();
            }
            while (!CheckInput(input, posibilities));
            return Convert.ToInt32(input);
        }
        public void Play()
        {
            while (!gameOver)
            {
                Output(questMap.GetLocationDescription());
                Output(player.GetCurrentState());
                ShowAvailableOptions(questMap.GetPossibleOptions());
                ProceedInput(GetConsoleInput(questMap.GetPossibleOptions()));
            }
        } 
        private void ProceedInput(int input)
        {
            switch (input)
            {
                case 0:
                    ClearOutdateInfo();
                    Output(questMap.Travel());
                    break;
                case 1:
                    questMap.ChangeTime(player.Rest());
                    break;
                case 2:
                    questMap.ChangeTime(player.Sleep());
                    break;
            }
        }
        public static void Output(string result)
        {
            Console.WriteLine(result);
        }
        private List<string> HardcodedSpotDescriptions()
        {
            List<string> descriptions = new List<string>();
            descriptions.Add("Просека. Много поваленных дереьев");
            descriptions.Add("Лес как лес. Кроме опавшей листвы ничего интересного");
            descriptions.Add("Старый дуб. Есть большое дупло, кажется, до него можно добраться");
            descriptions.Add("Смешанный лес. Много кустарника, есть ягоды");
            descriptions.Add("Преобладает хвоя приятно дышать полной грудью");
            descriptions.Add("Большая удача вы нашли ручей");
            descriptions.Add("Заросшее русло высохшей реки");
            descriptions.Add("Пещера. Выглядит довольно большой");
            descriptions.Add("Покинутая землянка. Дверь выглядит функционирующей, но замок рассыпался");
            descriptions.Add("Большая поляна");
            return descriptions;
        }
        public static void ClearOutdateInfo()
        {
            Console.Clear();
        }
        public Map CreateNewMap(int mapSize, Player player)
        {
            Spot exit;
            Map questMap = new Map(player);
            for (int i = 0; i <= mapSize; i++)
            {
                for (int j = 0; j <= mapSize; j++)
                {
                    questMap.AddSpot(new Spot((i, j), PickRandomDescription(HardcodedSpotDescriptions(), randomizer)));
                }
            }
            return questMap;
        }
        private Spot CreateExit()
        {
            int x = randomizer.Next(0, (int)MainQuestConfig.MapSize);
            int y;
            if( x == 0 || x == (int)MainQuestConfig.MapSize)
            {
                y = randomizer.Next(0, (int)MainQuestConfig.MapSize);
            }
            else
            {
                y = randomizer.Next(0, 1) * (int)MainQuestConfig.MapSize;
            }
        }
        static string PickRandomDescription(List<String> descriptions, Random randomizer)
        {
            return descriptions[randomizer.Next(0, descriptions.Count - 1)];
        }
    }
}
