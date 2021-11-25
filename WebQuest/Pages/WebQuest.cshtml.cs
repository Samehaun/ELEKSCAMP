using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ELEKSUNI;

namespace WebQuest.Pages
{
    //static class ActiveQuests
    //{
    //    public static Dictionary<Guid, Quest> quests = new Dictionary<Guid, Quest>();
    //}
    //public class WebQuestModel : PageModel
    //{
    //    public Guid id;
    //    private Quest quest;
    //    private Report state;
    //    public string Message { get; private set; }
    //    public string PlayerState { get; private set; }
    //    public List<string> Options { get; private set; }
    //    public void OnGet(string name)
    //    {
    //        id = Guid.NewGuid();
    //        quest = new Quest();
    //        state = quest.Start(name);
    //        Message = state.Message;
    //        PlayerState = state.PlayerState;
    //        Options = state.Options;
    //        id = Guid.NewGuid();
    //        ActiveQuests.quests.Add(id, quest);
    //    }
    //    public void OnPost(int selectedOptionId, Guid id)
    //    {
    //        this.id = id;
    //        quest = ActiveQuests.quests[id];
    //        state = quest.ProceedInput(selectedOptionId);
    //        Message = state.Message;
    //        PlayerState = state.PlayerState;
    //        Options = state.Options;
    //    }
    //}
    static class ActiveQuests
    {
        public static Dictionary<Guid, QuestState> quests = new Dictionary<Guid, QuestState>();
    }
    public class WebQuestModel : PageModel
    {
        public Guid id;
        private Quest quest;
        private Report state;
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
            ActiveQuests.quests.Add(id, quest.Save());
        }
        public void OnPost(int selectedOptionId, Guid id)
        {
            this.id = id;
            quest = new Quest();
            quest.Load(ActiveQuests.quests[id]);
            state = quest.ProceedInput(selectedOptionId);
            ActiveQuests.quests.Remove(id);
            ActiveQuests.quests.Add(id, quest.Save());
            Message = state.Message;
            PlayerState = state.PlayerState;
            Options = state.Options;
        }
    }
}
