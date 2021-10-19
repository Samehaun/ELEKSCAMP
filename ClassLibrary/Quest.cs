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

        public bool PlayerCanTravel()
        {
            return questMap.NotNightTime();
        }

        public void Rest()
        {
            questMap.ChangeTime(player.Rest());
        }

        public void Sleep()
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
    }
}
