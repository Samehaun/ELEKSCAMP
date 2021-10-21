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
            Console.WriteLine("Enter your name");
            quest = new Quest();
            quest.QuestOver += GameOver;
            quest.WaitingForInput += MakeAChoice;
            quest.Start(Console.ReadLine());
        }
        static void Main(string[] args)
        {
            ConsoleQuestLauncher launcher = new ConsoleQuestLauncher();
        }
        private static void ShowAvailableOptions(List<string> posibilities)
        {
            int i = 0;
            foreach (var option in posibilities)
            {
                Console.WriteLine($" { i++ } - { option }");
            }
        }
        private int GetInput(int maxPossibleInput)
        {
            string input;
            do
            {
                input = Console.ReadLine();
            }
            while (!CheckInput(input, maxPossibleInput));
            return Convert.ToInt32(input);
        }
        public void MakeAChoice((string message, string playerState, List<string> options) state)
        {
            Console.Clear();
            Output(state);
            quest.ProcceedInput(GetInput(state.options.Count));
        }
        private void GameOver(string endMessage)
        {
            Console.WriteLine(endMessage);
            Environment.Exit(0);
        }
        private void Output((string message, string state, List<string> options) questState)
        {
            Console.Clear();
            Console.WriteLine(questState.message);
            Console.WriteLine(questState.state);
            ShowAvailableOptions(questState.options);
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
