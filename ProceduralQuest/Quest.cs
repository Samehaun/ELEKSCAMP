using System;
using System.Collections.Generic;
using System.Text;

namespace ProceduralQuest
{
    class Quest
    {
        public Quest()
        {
            commands = new Dictionary<Keys, Action>() 
            { 
            };
        }
        public QuestState Start(string name)
        {
            return new QuestState();
        }
        public QuestState ProceedInput(int input)
        {
            return new QuestState();
        }
        private Dictionary<Keys, Action> commands;
        private void Go()
        {

        }

    }
}
