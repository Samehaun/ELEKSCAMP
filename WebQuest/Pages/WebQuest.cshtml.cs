using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ELEKSUNI;

namespace WebQuest.Pages
{
    public class WebQuestModel : PageModel
    {
        private Quest quest;
        private QuestState state;
        public string Message { get; private set; }
        public string PlayerState { get; private set; }
        public List<string> Options { get; private set; }
        public void OnGet(string name)
        {
            quest = new Quest();
            state = quest.Start(name);
            Message = state.Message;
            PlayerState = state.PlayerState;
            Options = state.Options;
        }
        public void OnPost(int selectedOptionId)
        {
            state = quest.ProcceedInput(selectedOptionId);
            Message = state.Message;
            PlayerState = state.PlayerState;
            Options = state.Options;
        }
    }
}
