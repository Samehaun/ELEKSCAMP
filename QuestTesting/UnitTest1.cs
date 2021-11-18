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
            quest.ProceedInput(0);
            quest.ProceedInput(0);
            var result = quest.ProceedInput(2);
            Assert.Equal($"You have reached new zone { Environment.NewLine } Road! It will lead somewhere. You got out!", result.Message);
        }
        [Fact]
        public void SearchWorks()
        {
            Quest quest = new Quest();
            quest.Start("Test");
            quest.ProceedInput(0);
            quest.ProceedInput(0);
            quest.ProceedInput(0);
            quest.ProceedInput(0);
            quest.ProceedInput(2);
            var result1 = quest.ProceedInput(4);
            quest.ProceedInput(1);
            quest.ProceedInput(3);
            var result2 = quest.ProceedInput(4);
            Assert.NotEqual(result1.Options.Count, result2.Options.Count);
        }
  

    }
}
