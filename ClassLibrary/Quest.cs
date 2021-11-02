using System;
using System.Collections.Generic;

namespace ELEKSUNI
{
    enum MainQuestConfig 
    {
        BasePlayerSpeed = 5,
        BasePlayerStaminaConsuption = 5,
        MaxWeigtPlayerCanCarry = 10,
        BaseTimeToChangeLocation = 3,
        MapSize = 4,
        MazeDifficulty = 2
    }
    public class Quest
    {
        private Map questMap;
        private Player player;
        public delegate QuestState InputHandler(int input);
        public InputHandler ProcceedInput;
        private string language;
        public bool IsEnded { get; private set; }
        private List<string> languages = new List<string>() { "EN", "RU", "UA" };
        private void Rest()
        {
            questMap.ChangeTime(player.Rest());
        }
        private void Sleep()
        {
            questMap.ChangeTime(player.Sleep());
        }
        private QuestState LocationMainDialog(int input)
        {
            string info;
            List<Keys> options = questMap.GetPossibleOptions();
            switch (input)
            {
                case 0:
                    info = Data.Localize(Keys.DirectionDialogMessage, language);
                    options = questMap.GetTravelDirections();
                    ProcceedInput = TravelDialog;
                    break;
                case 1:
                    Rest();
                    info = $"{Data.Localize(Keys.StaminaRecovered, language)} {Environment.NewLine} { Data.Localize(questMap.GetLocationDescription(), language) }";
                    break;
                case 2:
                    if (questMap.NotNightTime())
                    {
                        info = $"{Data.Localize(Keys.DayTimeSleep, language)} {Environment.NewLine} { Data.Localize(questMap.GetLocationDescription(), language) }";
                    }
                    else
                    {
                        Sleep();
                        info = $"{Data.Localize(Keys.WakeUp, language)} {Environment.NewLine} { Data.Localize(questMap.GetLocationDescription(), language) }";
                    }
                    break;
                case 3:
                    questMap.
                default:
                    info = "incorrect input";
                    break;
            }
            return new QuestState(info, Data.StateBuilder(player, language), Data.Localize(options, language));
        }
        public QuestState Start(string name)
        {
            player = new Player(name);
            questMap = new Map(player);
            questMap.SetPlayerLocation(((int)MainQuestConfig.MapSize / 2, (int)MainQuestConfig.MapSize / 2));
            ProcceedInput = SetLanguage;
            return new QuestState($"Select prefered language:", null, languages);         
        }
        private QuestState TravelDialog(int input)
        {
            string info = $"{ Data.Localize(questMap.Travel(questMap.GetTravelDirections()[input]), language) } {Environment.NewLine} {Data.Localize(questMap.GetLocationDescription(), language)}";
            ProcceedInput = LocationMainDialog;
            if (!questMap.ExitReached)
            {
                return new QuestState(info, Data.StateBuilder(player, language), Data.Localize(questMap.GetPossibleOptions(), language));
            }
            else
            {
                IsEnded = true;
                return new QuestState(info, null, null);
            }
        }
        private QuestState InventoryDialog(int input)
        {

        }
        private QuestState SetLanguage(int input)
        {
            switch (input)
            {
                case 0:
                    language = "EN";
                    break;
                case 1:
                    language = "RU";
                    break;
                case 2:
                    language = "UA";
                    break;
                default:
                    break;
            }
            string initialMessage = Data.Localize(Keys.InitialMessage, language);
            ProcceedInput = LocationMainDialog;
            return new QuestState($"{ initialMessage } {Environment.NewLine} { Data.Localize(questMap.GetLocationDescription(), language) }",
                Data.StateBuilder(player, language), Data.Localize(questMap.GetPossibleOptions(), language));
        }
    }

}
