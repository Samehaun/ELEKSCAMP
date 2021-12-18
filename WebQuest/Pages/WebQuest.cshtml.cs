using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ELEKSUNI;

namespace WebQuest
{
    //static class ActiveQuests
    //{
    //    public static Dictionary<Guid, QuestState> quests = new Dictionary<Guid, QuestState>();
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
    //        quest = new Quest();
    //        state = quest.Start(name);
    //        Message = state.Message;
    //        PlayerState = state.PlayerState;
    //        Options = state.Options;
    //        id = Guid.NewGuid();
    //        ActiveQuests.quests.Add(id, quest.Save());
    //    }
    //    public void OnPost(int selectedOptionId, Guid id)
    //    {
    //        this.id = id;
    //        quest = new Quest();
    //        quest.Load(ActiveQuests.quests[id]);
    //        state = quest.ProceedInput(selectedOptionId);
    //        if (!quest.IsEnded)
    //        {
    //            ActiveQuests.quests.Remove(id);
    //            ActiveQuests.quests.Add(id, quest.Save());
    //        }
    //        Message = state.Message;
    //        PlayerState = state.PlayerState;
    //        Options = state.Options;
    //    }
    //}
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
            AssignTextValues();
            id = Guid.NewGuid();
            SaveToDB();
        }
        public void OnPost(int selectedOptionId, Guid id)
        {
            this.id = id;
            quest = new Quest();
            using (ApplicationContext db = new ApplicationContext())
            {
                QuestState save = db.Quests.Find(id);
                quest.Load(save);
                state = quest.ProceedInput(selectedOptionId);
                AssignTextValues();
                if (quest.IsEnded)
                {
                    db.Quests.Remove(save);
                }
                else
                {
                    UpdateQuestRecord(db, save);
                }
                db.SaveChanges();
            }
            void UpdateQuestRecord(ApplicationContext db, QuestState save)
            {
                QuestState update = quest.Save();
                save.History = update.History;
                save.Map = update.Map;
                save.Player = update.Player;
                save.Report = update.Report;
                save.Time = update.Time;
                save.Options = update.Options;
            }
        }
        private void SaveToDB()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                QuestState save = quest.Save();
                save.Id = id;
                db.Quests.Add(save);
                db.SaveChanges();
            }
        }
        private void AssignTextValues()
        {
            Message = state.Message;
            PlayerState = state.PlayerState;
            Options = state.Options;
        }
    }
}
