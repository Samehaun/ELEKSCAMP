using System;
using Xunit;
using ELEKSUNI;

namespace QuestTesting
{
    public class UnitTest1
    {
        [Fact]
        public void ExitWorks()
        {
            Quest quest = new Quest();
            Report result = quest.Start("Test");
            result = quest.ProceedInput(GetIndexOfCorrespondinOption(result, "EN"));
            result = GoToExit(quest, result);
            Assert.Equal($"You have reached new zone{ Environment.NewLine }Road! It will lead somewhere. You got out!", result.Message);

            Report GoToExit(Quest quest, Report result)
            {
                result = quest.ProceedInput(GetIndexOfCorrespondinOption(result, "Walk"));
                result = quest.ProceedInput(GetIndexOfCorrespondinOption(result, "West"));
                return result;
            }
        }
        [Fact]
        public void SearchCommandWorks()
        {
            Quest quest = new Quest();
            Report result = quest.Start("Test");
            result = quest.ProceedInput(GetIndexOfCorrespondinOption(result, "EN"));
            result = GoToSpotWithHiddenItem(quest, result);
            var result1 = quest.ProceedInput(GetIndexOfCorrespondinOption(result, "Inventory")).Options.Count;
            result = quest.ProceedInput(GetIndexOfCorrespondinOption(result, "Cancel"));
            result = quest.ProceedInput(GetIndexOfCorrespondinOption(result, "Search"));
            var result2 = quest.ProceedInput(GetIndexOfCorrespondinOption(result, "Inventory")).Options.Count;
            Assert.True(result1 < result2);

            Report GoToSpotWithHiddenItem(Quest quest, Report result)
            {
                result = quest.ProceedInput(GetIndexOfCorrespondinOption(result, "Walk"));
                result = quest.ProceedInput(GetIndexOfCorrespondinOption(result, "East"));
                result = quest.ProceedInput(GetIndexOfCorrespondinOption(result, "Walk"));
                result = quest.ProceedInput(GetIndexOfCorrespondinOption(result, "South"));
                return result;
            }
        }
        [Fact]
        public void SaveLoadSimpleTest()
        {
            Quest quest = new Quest();
            Report result = quest.Start("Test");
            quest = SaveLoad(quest);
            result = quest.ProceedInput(GetIndexOfCorrespondinOption(result, "EN"));
            quest = SaveLoad(quest);
            result = quest.ProceedInput(GetIndexOfCorrespondinOption(result, "Walk"));
            quest = SaveLoad(quest);
            result = quest.ProceedInput(GetIndexOfCorrespondinOption(result, "West"));
            Assert.Equal($"You have reached new zone{ Environment.NewLine }Road! It will lead somewhere. You got out!", result.Message);
        }
        [Fact]
        public void SaveLoadComplexTest()
        {
            Quest quest = new Quest();
            Report result = quest.Start("Test"); 
            quest = SaveLoad(quest);
            result = quest.ProceedInput(GetIndexOfCorrespondinOption(result, "EN"));
            quest = SaveLoad(quest);
            result = quest.ProceedInput(GetIndexOfCorrespondinOption(result, "Walk"));
            quest = SaveLoad(quest);
            result = quest.ProceedInput(GetIndexOfCorrespondinOption(result, "East"));
            quest = SaveLoad(quest);
            result = quest.ProceedInput(GetIndexOfCorrespondinOption(result, "Walk"));
            quest = SaveLoad(quest);
            result = quest.ProceedInput(GetIndexOfCorrespondinOption(result, "South"));
            quest = SaveLoad(quest);
            result = quest.ProceedInput(GetIndexOfCorrespondinOption(result, "Inventory"));
            var result1 = result.Options.Count;
            quest = SaveLoad(quest);
            result = quest.ProceedInput(GetIndexOfCorrespondinOption(result, "Cancel"));
            quest = SaveLoad(quest);
            result = quest.ProceedInput(GetIndexOfCorrespondinOption(result, "Search"));
            quest = SaveLoad(quest);
            var result2 = quest.ProceedInput(GetIndexOfCorrespondinOption(result, "Inventory")).Options.Count;
            Assert.True(result1 < result2);
        }
        Quest SaveLoad (Quest quest)
        {
            var save = quest.Save();
            Quest newQuest = new Quest();
            newQuest.Load(save);
            return newQuest;
        }
        int GetIndexOfCorrespondinOption(Report options, string toFind)
        {
            return options.Options.FindIndex(t => t.Contains(toFind));
        }
    }
}
