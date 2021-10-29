using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ELEKSUNI;

namespace WebQuest.Pages
{
    static class ActiveQuests
    {
        public static Dictionary<Guid, Quest> quests = new Dictionary<Guid, Quest>();
    }
    public class WebQuestModel : PageModel
    {
        private static Guid id;
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
            id = Guid.NewGuid();
            ActiveQuests.quests.Add(id, quest);
        }
        public void OnPost(int selectedOptionId)
        {
            quest = ActiveQuests.quests[id];
            state = quest.ProcceedInput(selectedOptionId);
            Message = state.Message;
            PlayerState = state.PlayerState;
            Options = state.Options;
        }
    }
}
