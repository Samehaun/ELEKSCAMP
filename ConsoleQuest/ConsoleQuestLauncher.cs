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
            quest.OnPlayerTravel += Travel;
            quest.OnPlayerRest += Rest;
            quest.OnPlayerSleep += Sleep;
            quest.OnDayTimeSleep += ImpossibleToSleepAtDayTime;
            quest.OnPlayerReachedNewSpot += InteractWithLocation;
            Output("Вы пришли в себя в незнакомом месте. Неизвестно как вы здесь оказались, но по крайней мере вы живы и здоровы... пока");
        }

        private void ImpossibleToSleepAtDayTime()
        {
            ClearOutdateInfo();
            Output($"Спать днем?! А что собираетесь делать ночью?");
        }

        private void Sleep()
        {
            ClearOutdateInfo();
            Output("Вы полны сил");
        }

        private void Rest()
        {
            ClearOutdateInfo();
            Output("Вы немного отдохнули");
        }

        private void Travel()
        {
            Output(TravelMenu());
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
            quest.ProceedInput(GetInput(quest.GetPossibleOptions()));
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
            ClearOutdateInfo();
            Output("Выберите направление");
            ShowAvailableOptions(quest.GetTravelDirections());
            input = quest.GetTravelDirections()[GetInput(quest.GetTravelDirections())];
            ClearOutdateInfo();
            return quest.Travel(input);
        }
    }
}
