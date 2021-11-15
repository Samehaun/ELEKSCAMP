using System;
using Xunit;
using ELEKSUNI;

namespace QuestTesting
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            Quest quest = new Quest();
            var result = quest.Start("Test");
            Assert.Null(result.PlayerStateOrAdditionalInformation);
        }
    }
}
