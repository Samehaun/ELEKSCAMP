using System;
using System.Collections.Generic;
using ELEKSUNI;

namespace ConsoleQuest
{
    class ConsoleQuestLauncher
    {
        Quest quest;
        public ConsoleQuestLauncher()
        {
            Output("Enter your name");
            quest = new Quest(Console.ReadLine());
            ClearOutdateInfo();
            quest.GameOver += GameOver;
            Output("Вы пришли в себя в незнакомом месте. Неизвестно как вы здесь оказались, но по крайней мере вы живы и здоровы... пока");
        }
        static void Main(string[] args)
        {
            ConsoleQuestLauncher quest = new ConsoleQuestLauncher();
            quest.InteractWithLocation();
        }
        public static void ShowAvailableOptions(List<string> posibilities)
        {
            int i = 0;
            foreach (var option in posibilities)
            {
                Output($" { i++ } - { option }");
            }
        }
        public static int GetInput(List<string> posibilities)
        {
            string input;
            do
            {
                input = Console.ReadLine();
            }
            while (!Quest.CheckInput(input, posibilities));
            return Convert.ToInt32(input);
        }
        public void InteractWithLocation()
        {
            Output(quest.GetLocationDescription());
            Output(quest.GetCurrentPlayerState());
            ShowAvailableOptions(quest.GetPossibleOptions());
            ProceedInput(GetInput(quest.GetPossibleOptions()));
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
                    quest.Rest();
                    break;
                case 2:
                    ClearOutdateInfo();
                    if (quest.PlayerCanTravel())
                    {
                        Output($"Спать днем?! А что собираетесь делать ночью?");
                    }
                    else
                    {
                        Output("Вы полны сил");
                        quest.Sleep();
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
            Output(quest.GetLocationDescription());
            Environment.Exit(0);
        }
        public string TravelMenu()
        {
            string input;
            Output("Выберите направление");
            ShowAvailableOptions(quest.GetTravelDirections());
            input = quest.GetTravelDirections()[GetInput(quest.GetTravelDirections())];
            ClearOutdateInfo();
            return quest.Travel(input);
        }
    }
}
