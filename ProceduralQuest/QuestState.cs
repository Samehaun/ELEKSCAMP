using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProceduralQuest
{
    public class QuestState
    {
        public string Message { get; private set; }
        public string PlayerState { get; private set; }
        public List<string> Options { get; private set; }
        public QuestState(string message, string playerState, List<string> options)
        {
            Message = message;
            PlayerState = playerState;
            Options = options;
        }
    }
}
