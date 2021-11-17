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
            quest.ProceedInput(1);
            var result = quest.ProceedInput(3);
            Assert.Equal($"You have reached new zone {Environment.NewLine} Road! It will lead somewhere.You got out !", result.Message);
        }

    }
}
