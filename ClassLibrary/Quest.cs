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
        Map questMap;
        Player player;
        public delegate void EndHandler(string message);
        public delegate void QuestHandler((string message, string playerState, List<string> options) state);
        public delegate void InputHandler(int input);
        public InputHandler ProcceedInput;
        public event EndHandler QuestOver;
        public event QuestHandler WaitingForInput;

        private void EndQuest()
        {
            QuestOver?.Invoke(questMap.GetLocationDescription());
        }
        private void Rest()
        {
            questMap.ChangeTime(player.Rest());
        }
        private void Sleep()
        {
            questMap.ChangeTime(player.Sleep());
        }
        private void LocationMainDialog(int input)
        {
            string info;
            List<string> options = questMap.GetPossibleOptions();
            switch (input)
            {
                case 0:
                    info = "Выберите направление";
                    options = questMap.GetTravelDirections();
                    ProcceedInput = TravelDialog;
                    break;
                case 1:
                    Rest();
                    info = $"Вы немного отдохнули {Environment.NewLine} { questMap.GetLocationDescription() }";
                    break;
                case 2:
                    if (questMap.NotNightTime())
                    {
                        info = $"Спать днем?! А что собираетесь делать ночью? {Environment.NewLine} { questMap.GetLocationDescription() }";
                    }
                    else
                    {
                        Sleep();
                        info = $"Вы полны сил {Environment.NewLine} { questMap.GetLocationDescription() }";
                    }
                    break;
                default:
                    info = "incorrect input";
                    break;
            }
            WaitingForInput?.Invoke((info, player.GetCurrentState(), options));
        }
        public void Start(string name)
        {
            string initialMessage = "Вы пришли в себя в незнакомом месте. Неизвестно как вы здесь оказались, но по крайней мере вы живы и здоровы... пока";
            player = new Player(name);
            questMap = new Map(player);
            questMap.SetPlayerLocation(((int)MainQuestConfig.MapSize / 2, (int)MainQuestConfig.MapSize / 2));
            questMap.PlayerReachedExit += EndQuest;
            ProcceedInput = LocationMainDialog; 
            WaitingForInput?.Invoke((initialMessage, player.GetCurrentState(), questMap.GetPossibleOptions()));
        }
        private void TravelDialog(int input)
        {
            string info = questMap.Travel(questMap.GetTravelDirections()[input]);
            ProcceedInput = LocationMainDialog;
            WaitingForInput?.Invoke((info, player.GetCurrentState(), questMap.GetPossibleOptions()));
        }
    }
}
