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
        Dictionary<Keys, Action> commands;
        Time time;
        Map questMap;
        Player player;
        private List<Keys> availableCommands;
        public delegate QuestState InputHandler(int input);
        public InputHandler ProcceedInput;
        private string language;
        public bool IsEnded { get; private set; }
        private List<string> languages = new List<string>() { "EN", "RU", "UA" };
        QuestState state;
        private QuestState LocationMainDialog(int input)
        {
            commands[availableCommands[input]].Invoke();
            return state;
        }
        public QuestState Start(string name)
        {
            player = new Player(name);
            questMap = new Map(player);
            time = new Time();
            state = new QuestState();
            commands = new Dictionary<Keys, Action>()
            {
                { Keys.North, GoNorth },
                { Keys.South, GoSouth },
                { Keys.East, GoEast },
                { Keys.West, GoWest },
                { Keys.Travel, Travel},
                { Keys.Sleep, Sleep},
                { Keys.Rest, Rest}
            };
            availableCommands = new List<Keys>();
            questMap.SetPlayerLocation(((int)MainQuestConfig.MapSize / 2, (int)MainQuestConfig.MapSize / 2));
            ProcceedInput = SetLanguage;
            state.Message = $"Select prefered language:";
            state.Options = languages;
            return state;         
        }
        private QuestState TravelDialog(int input)
        {
            commands[availableCommands[input]].Invoke();
            if (!questMap.ExitReached)
            {
                state.PlayerState = Data.StateBuilder(player, language);
            }
            else
            {
                IsEnded = true;
                state.PlayerState = null;
                state.Options = null;
            }
            ProcceedInput = LocationMainDialog;
            return state;
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
            state.Message = $"{ initialMessage } {Environment.NewLine} { Data.Localize(questMap.GetLocationDescription(), language) }";
            state.PlayerState = Data.StateBuilder(player, language);
            availableCommands.AddRange(questMap.GetPossibleOptions());
            state.Options = Data.Localize(availableCommands, language);
            return state;
        }
        private void EN()
        {

        }
        private void Sleep()
        {
            if (time.NotNightTime())
            {
                state.Message = $"{Data.Localize(Keys.DayTimeSleep, language)} {Environment.NewLine} { Data.Localize(questMap.GetLocationDescription(), language) }";
            }
            else
            {
                time.ChangeTime(player.Sleep());
                state.Message = $"{Data.Localize(Keys.WakeUp, language)} {Environment.NewLine} { Data.Localize(questMap.GetLocationDescription(), language) }";
            }
        }
        private void Rest()
        {
            time.ChangeTime(player.Rest());
            state.Message = $"{Data.Localize(Keys.StaminaRecovered, language)} {Environment.NewLine} { Data.Localize(questMap.GetLocationDescription(), language) }";
        }
        private void GoNorth()
        {
            questMap.Go(Keys.North);
            NewZone();
        }
        private void GoSouth()
        {
            questMap.Go(Keys.South);
            NewZone();
        }
        private void GoEast()
        {
            questMap.Go(Keys.South);
            NewZone();
        }
        private void GoWest()
        {
            questMap.Go(Keys.South);
            NewZone();
        }
        private void Travel()
        {
            if (time.NotNightTime())
            {
                state.Message = Data.Localize(Keys.DirectionDialogMessage, language);
                availableCommands.Clear();
                availableCommands.AddRange(questMap.GetTravelDirections());
                ProcceedInput = TravelDialog;
            }
            else
            {
                state.Message = Data.Localize(Keys.NightTime, language);
            }
            state.PlayerState = Data.StateBuilder(player, language);
        }
        private void NewZone()
        {
            state.Message = $"{ Data.Localize(Keys.NextZone, language) } { Environment.NewLine } { Data.Localize(questMap.GetLocationDescription(), language) }";
            time.ChangeTime(player.CalculateTimeNeededToTravel());
            availableCommands.Clear();
            availableCommands.AddRange(questMap.GetPossibleOptions());
            player.RecaculateStateDueToTraveling();
            state.PlayerState = Data.StateBuilder(player, language);
            state.Options = Data.Localize(availableCommands, language);
        }
    }
}
