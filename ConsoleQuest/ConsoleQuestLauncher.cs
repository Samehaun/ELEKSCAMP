using System;
using System.Collections.Generic;
using ELEKSUNI;

namespace ConsoleQuest
{
    class ConsoleQuestLauncher
    {
        Quest quest;
        QuestState state;
        public ConsoleQuestLauncher()
        {
            Console.WriteLine("Enter your name");
            quest = new Quest();
            quest.QuestOver += GameOver;
            state = quest.Start(Console.ReadLine());
        }
        static void Main(string[] args)
        {
            ConsoleQuestLauncher launcher = new ConsoleQuestLauncher();
            while(true)
            {
                ShowState(launcher.state);
                launcher.state = launcher.quest.ProcceedInput(GetInput(launcher.state.Options.Count));
            }
        }
        private static void ShowAvailableOptions(List<string> posibilities)
        {
            int i = 0;
            foreach (var option in posibilities)
            {
                Console.WriteLine($" { i++ } - { option }");
            }
        }
        public static int GetInput(int maxPossibleInput)
        {
            string input;
            do
            {
                input = Console.ReadLine();
            }
            while (!CheckInput(input, maxPossibleInput));
            return Convert.ToInt32(input);
        }
        public static void ShowState(QuestState state)
        {
            Console.Clear();
            Output(state);
        }
        private void GameOver(string endMessage)
        {
            Console.WriteLine(endMessage);
            Environment.Exit(0);
        }
        private static void Output(QuestState state)
        {
            Console.WriteLine(state.Message);
            Console.WriteLine(state.PlayerState);
            ShowAvailableOptions(state.Options);
        }
        private static bool CheckInput(string input, int maxPossibleInput)
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
            if (inputNumber >= 0 && inputNumber < maxPossibleInput)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
