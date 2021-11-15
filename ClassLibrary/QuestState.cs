using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ELEKSUNI
{
    public class QuestState
    {
        public string Message { get; set; }
        public string PlayerStateOrAdditionalInformation { get; set; }
        public List<string> Options { get; set; }
        public QuestState()
        {
            Options = new List<string>();
        }
    }
}