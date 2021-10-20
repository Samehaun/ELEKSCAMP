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
        public delegate void QuestHandler();
        public event QuestHandler GameOver;
        public event QuestHandler OnPlayerTravel;
        public event QuestHandler OnPlayerSleep;
        public event QuestHandler OnPlayerRest;
        public event QuestHandler OnDayTimeSleep;
        public event QuestHandler OnPlayerReachedNewSpot;
        public Quest(string playerName)
        {
            player = new Player(playerName);
            questMap = new Map(player);
            questMap.SetPlayerLocation(((int)MainQuestConfig.MapSize / 2, (int)MainQuestConfig.MapSize / 2));
            questMap.PlayerReachedExit += Victory;
        }
        private void Victory()
        {
            GameOver?.Invoke();
        }
        public string GetLocationDescription()
        {
            return questMap.GetLocationDescription();
        }
        public string GetCurrentPlayerState()
        {
            return player.GetCurrentState();
        }
        public List<string> GetPossibleOptions()
        {
            return questMap.GetPossibleOptions();
        }
        private void Rest()
        {
            questMap.ChangeTime(player.Rest());
        }
        private void Sleep()
        {
            questMap.ChangeTime(player.Sleep());
        }
        public List<string> GetTravelDirections()
        {
            return questMap.GetTravelDirections();
        }
        public string Travel(string input)
        {
            return questMap.Travel(input);
        }
        public static bool CheckInput(string input, List<string> posibilities)
        {
            int inputNumber;
            try
            {
                inputNumber = Convert.ToInt32(input);
            }
            catch (Exception)
            {
                return false;
            }
            if (inputNumber >= 0 && inputNumber < posibilities.Count)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public void ProceedInput(int input)
        {
            switch (input)
            {
                case 0:
                    OnPlayerTravel?.Invoke();
                    break;
                case 1:
                    Rest();
                    OnPlayerRest?.Invoke();
                    break;
                case 2:
                    if (questMap.NotNightTime())
                    {
                        OnDayTimeSleep?.Invoke();
                    }
                    else
                    {
                        Sleep();
                        OnPlayerSleep?.Invoke();
                    }
                    break;
            }
            OnPlayerReachedNewSpot?.Invoke();
        }
    }
}
