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
            quest.Start("Test");
            Report result = GoToExit(quest);
            Assert.Equal($"You have reached new zone{ Environment.NewLine }Road! It will lead somewhere. You got out!", result.Message);
            static Report GoToExit(Quest quest)
            {
                quest.ProceedInput(0);
                quest.ProceedInput(0);
                var result = quest.ProceedInput(2);
                return result;
            }
        }
        [Fact]
        public void SearchCommandWorks()
        {
            Quest quest = new Quest();
            quest.Start("Test");
            GoToSpotWithHiddenItem(quest);
            quest.ProceedInput(2);
            var result1 = quest.ProceedInput(4).Options.Count;
            quest.ProceedInput(1);
            quest.ProceedInput(3);
            var result2 = quest.ProceedInput(4).Options.Count;
            Assert.True(result1 < result2);

            static void GoToSpotWithHiddenItem(Quest quest)
            {
                quest.ProceedInput(0);
                quest.ProceedInput(0);
                quest.ProceedInput(0);
                quest.ProceedInput(0);
            }
        }
        [Fact]
        public void SaveLoadSimpleTest()
        {
            Quest quest = new Quest();
            quest.Start("Test");
            quest = SaveLoad(quest);
            quest.ProceedInput(0);
            quest = SaveLoad(quest);
            quest.ProceedInput(0);
            quest = SaveLoad(quest);
            var result = quest.ProceedInput(2);
            Assert.Equal($"You have reached new zone{ Environment.NewLine }Road! It will lead somewhere. You got out!", result.Message);
        }
        [Fact]
        public void SaveLoadComplexTest()
        {
            Quest quest = new Quest();
            quest.Start("Test");
            quest = SaveLoad(quest);
            quest.ProceedInput(0);
            quest = SaveLoad(quest);
            quest.ProceedInput(0);
            quest = SaveLoad(quest);
            quest.ProceedInput(0);
            quest = SaveLoad(quest);
            quest.ProceedInput(0);
            quest = SaveLoad(quest);
            quest.ProceedInput(2);
            quest = SaveLoad(quest);
            var result1 = quest.ProceedInput(4).Options.Count;
            quest = SaveLoad(quest);
            quest.ProceedInput(1);
            quest = SaveLoad(quest);
            quest.ProceedInput(3);
            quest = SaveLoad(quest);
            var result2 = quest.ProceedInput(4).Options.Count;
            Assert.True(result1 < result2);
        }
        Quest SaveLoad (Quest quest)
        {
            var save = quest.Save();
            Quest newQuest = new Quest();
            newQuest.Load(save);
            return newQuest;
        }



    }
}
