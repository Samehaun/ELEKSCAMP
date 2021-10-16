using System;
using System.Collections.Generic;
using ELEKSUNI;

namespace ConsoleQuest
{
    class ConsoleQuestLauncher
    {
        Player player;
        Map questMap;
        public ConsoleQuestLauncher()
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
            ConsoleQuestLauncher quest = new ConsoleQuestLauncher();
            quest.InteractWithLocation();
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
            if (inputNumber >= 0 && inputNumber < posibilities.Count)
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
        public void InteractWithLocation()
        {
            Output(questMap.GetLocationDescription());
            Output(player.GetCurrentState());
            ShowAvailableOptions(questMap.GetPossibleOptions());
            ProceedInput(GetInput(questMap.GetPossibleOptions()));
        }
        private void ProceedInput(int input)
        {
            switch (input)
            {
                case 0:
                    ClearOutdateInfo();
                    Output(TravelMenu());
                    break;
                case 1:
                    ClearOutdateInfo();
                    Output("Вы немного отдохнули");
                    questMap.ChangeTime(player.Rest());
                    break;
                case 2:
                    ClearOutdateInfo();
                    if (questMap.NotNightTime())
                    {
                        Output($"Спать днем?! А что собираетесь делать ночью?");
                    }
                    else
                    {
                        Output("Вы полны сил");
                        questMap.ChangeTime(player.Sleep());
                    }
                    break;
            }
            InteractWithLocation();
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
        public string TravelMenu()
        {
            string input;
            Output("Выберите направление");
            ShowAvailableOptions(questMap.GetTravelDirections());
            input = questMap.GetTravelDirections()[GetInput(questMap.GetTravelDirections())];
            ClearOutdateInfo();
            return questMap.Travel(input);
        }
    }
}
