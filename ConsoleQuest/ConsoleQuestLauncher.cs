using System;
using System.Collections.Generic;
using ELEKSUNI;

namespace ConsoleQuest
{
    class ConsoleQuestLauncher
    {
        Quest quest;
        Report state;
        public ConsoleQuestLauncher()
        {
            Console.WriteLine("Enter your name");
            quest = new Quest();
            state = quest.Start(Console.ReadLine());
        }
        static void Main(string[] args)
        {
            ConsoleQuestLauncher launcher = new ConsoleQuestLauncher();
            do
            {
                ShowState(launcher.state);
                launcher.state = launcher.quest.ProceedInput(GetInput(launcher.state.Options.Count));
            } while (!launcher.quest.IsEnded);
            ShowState(launcher.state);
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
        public static void ShowState(Report state)
        {
            Console.Clear();
            Output(state);
        }
        private static void Output(Report state)
        {
            Console.WriteLine(state.Message);
            if (state.PlayerState != null)
            {
                Console.WriteLine(state.PlayerState);
            }
            if (state.Options != null)
            {
                ShowAvailableOptions(state.Options);
            }
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