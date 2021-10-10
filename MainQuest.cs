using System;
using System.Collections.Generic;

namespace ELEKSUNI
{
    enum MainQuestConfig 
    {
        BasePlayerSpeed = 5,
        BasePlayerStaminaConsuption = 5,
        MaxWeigtPlayerCanCarry = 10,
        BaseTimeToChangeLocation = 3,
        MapSize = 4,
        MazeDifficulty = 2
    }
    class MainQuest
    {
        Player player;
        Map questMap;
        public MainQuest()
        {
            player = CreatePlayer();
            questMap = new Map(player);
            questMap.SetPlayerLocation(((int)MainQuestConfig.MapSize / 2, (int)MainQuestConfig.MapSize / 2));
            ClearOutdateInfo();
            questMap.PlayerReachedExit += GameOver;
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
        public static int GetInput(List<string> posibilities)
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
            while (true)
            {
                Output(questMap.GetLocationDescription());
                Output(player.GetCurrentState());
                ShowAvailableOptions(questMap.GetPossibleOptions());
                ProceedInput(GetInput(questMap.GetPossibleOptions()));
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
                    if(questMap.NotNightTime())
                    {
                        Output($"Спать днем?! А что собираетесь делать ночью?");
                        break;
                    }
                    else
                    {
                        questMap.ChangeTime(player.Sleep());
                        break;
                    }
            }
        }
        public static void Output(string result)
        {
            Console.WriteLine(result);
        }
        public static void ClearOutdateInfo()

        {
            Console.Clear();
        }
        private void GameOver()
        {
            Output(questMap.GetLocationDescription());
            System.Environment.Exit(0);
        }
    }
}
