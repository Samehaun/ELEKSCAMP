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
            Report result = quest.StartTestMode();
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
            Report result = quest.StartTestMode();
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
            Report result = quest.StartTestMode();
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
            Report result = quest.StartTestMode();
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
        [Fact]
        public void EnglishLocalizationWorks()
        {
            Quest quest = new Quest();
            Report result = quest.StartTestMode();
            result = quest.ProceedInput(GetIndexOfCorrespondinOption(result, "EN"));
            Assert.Contains("You have come to your senses in an unfamiliar place", result.Message);
        }

        [Fact]
        public void RussianLocalizationWorks()
        {
            Quest quest = new Quest();
            Report result = quest.StartTestMode();
            result = quest.ProceedInput(GetIndexOfCorrespondinOption(result, "RU"));
            Assert.Contains("Вы пришли в себя в незнакомом месте. Неизвестно как вы здесь оказались", result.Message);
        }
        [Fact]
        public void UkrainianLocalizationWorks()
        {
            Quest quest = new Quest();
            Report result = quest.StartTestMode();
            result = quest.ProceedInput(GetIndexOfCorrespondinOption(result, "UA"));
            Assert.Contains("Ви живі і здорові", result.Message);
        }
        [Fact]
        public void PlayerHasInitialArmor()
        {
            Quest quest = new Quest();
            Report result = quest.StartTestMode();
            result = quest.ProceedInput(GetIndexOfCorrespondinOption(result, "EN"));
            result = quest.ProceedInput(GetIndexOfCorrespondinOption(result, "Inventory"));
            Assert.Contains("Simple clothes", result.Options[0]);
        }
        [Fact]
        public void DropItemWorks()
        {
            Quest quest = new Quest();
            Report result = quest.StartTestMode();
            result = quest.ProceedInput(GetIndexOfCorrespondinOption(result, "EN"));
            result = quest.ProceedInput(GetIndexOfCorrespondinOption(result, "Inventory"));
            result = quest.ProceedInput(GetIndexOfCorrespondinOption(result, "Simple"));
            result = quest.ProceedInput(GetIndexOfCorrespondinOption(result, "Drop"));
            Assert.DoesNotContain("Simple clothes", result.Options[0]);
        }
        [Fact]
        public void PoisonedFoodWorks()
        {
            Quest quest = new Quest();
            Report result = quest.StartTestMode();
            result = quest.ProceedInput(GetIndexOfCorrespondinOption(result, "EN"));
            result = quest.ProceedInput(GetIndexOfCorrespondinOption(result, "Search"));
            result = quest.ProceedInput(GetIndexOfCorrespondinOption(result, "Inventory"));
            result = quest.ProceedInput(GetIndexOfCorrespondinOption(result, "Mushrooms"));
            result = quest.ProceedInput(GetIndexOfCorrespondinOption(result, "Eat"));
            Assert.Contains("poison", result.PlayerState);
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
