using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ELEKSUNI
{
    public class QuestState
    {
        public DateTime QuestTime { get;  }
        public Stack<Action> MenuCallHistory { get; }
        public QuestState(DateTime time, Stack<Action> stack)
        {
            QuestTime = time;
            MenuCallHistory = stack;
        }
        
    }
}
