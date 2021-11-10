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
        private string language;
        public bool IsEnded { get; private set; }
        QuestState state;
        public delegate QuestState InputHandler(int input);
        public InputHandler ProceedInput;
        public QuestState NonInventoryDialog(int input)
        {
            commands[availableCommands[input]].Invoke();
            return state;
        }
        public QuestState InventoryDialog(int input)
        {




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
                { Keys.Rest, Rest},
                { Keys.EN, EN},
                { Keys.RU, RU},
                { Keys.UA, UA},
                { Keys.Drop, player.inventory.Drop },
                { Keys.Sell, player.inventory.Sell },
                { Keys.Buy, player.inventory.Buy },
                { Keys.Equip, Equip },
                { Keys.Search, Search },
                { Keys.Inventory, ShowInventory }
            };
            availableCommands = new List<Keys>();
            questMap.SetPlayerLocation(((int)MainQuestConfig.MapSize / 2, (int)MainQuestConfig.MapSize / 2));
            state.Message = $"Select prefered language:";
            availableCommands.AddRange(new List<Keys>() { Keys.EN, Keys.RU, Keys.UA });
            state.Options.AddRange(new List<string>() { "EN", "RU", "UA" });
            ProceedInput = NonInventoryDialog;
            return state;
        }
        private void MainDialog()
        {
            if (!questMap.ExitReached)
            {
                state.PlayerState = Data.StateBuilder(player, language);
                ResetOptions(questMap.GetPossibleOptions());
            }
            else
            {
                IsEnded = true;
                state.PlayerState = null;
                state.Options = null;
            }
        }
        private void ResetOptions(List<Keys> options)
        {
            availableCommands.Clear();
            availableCommands.AddRange(options);
            state.Options.Clear();
            state.Options.AddRange(Data.Localize(options, language));
        }
        private void ResetOptions(List<string> options)
        {
            state.Options.Clear();
            state.Options.AddRange(options);
        }
        private void Start()
        {
            string initialMessage = Data.Localize(Keys.InitialMessage, language);
            state.Message = $"{ initialMessage } {Environment.NewLine} { Data.Localize(questMap.GetLocationDescription(), language) }";
            MainDialog();
        }
        private void EN()
        {
            language = "EN";
            Start();
        }
        private void RU()
        {
            language = "RU";
            Start();
        }
        private void UA()
        {
            language = "UA";
            Start();
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
                state.PlayerState = Data.StateBuilder(player, language);
            }
        }
        private void Rest()
        {
            time.ChangeTime(player.Rest());
            state.Message = $"{Data.Localize(Keys.StaminaRecovered, language)} {Environment.NewLine} { Data.Localize(questMap.GetLocationDescription(), language) }";
            state.PlayerState = Data.StateBuilder(player, language);
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
            questMap.Go(Keys.East);
            NewZone();
        }
        private void GoWest()
        {
            questMap.Go(Keys.West);
            NewZone();
        }
        private void Travel()
        {
            if (time.NotNightTime())
            {
                state.Message = Data.Localize(Keys.DirectionDialogMessage, language);
                ResetOptions(questMap.GetTravelDirections());
            }
            else
            {
                state.Message = $"{ Data.Localize(Keys.NightTime, language) } { Environment.NewLine } { Data.Localize(questMap.GetLocationDescription(), language) }";
            }
        }
        private void NewZone()
        {
            state.Message = $"{ Data.Localize(Keys.NextZone, language) } { Environment.NewLine } { Data.Localize(questMap.GetLocationDescription(), language) }";
            time.ChangeTime(player.CalculateTimeNeededToTravel());
            player.RecaculateStateDueToTraveling();
            MainDialog();
        }
        private void Equip()
        {
            if (player.inventory.CurrentItem is Weapon)
            {
                player.CurrentWeapon = (Weapon)player.inventory.CurrentItem;
            }
            else
            {
                player.CurrentClothes = (Clothes)player.inventory.CurrentItem;
            }
        }
        private void Search()
        {
            Item newItem = questMap.PlayerSpot.item;
            if (newItem != null)
            {
                player.inventory.Add(newItem);
                questMap.PlayerSpot.item = null;
            }
        }
        private void Select(int input)
        {
            player.inventory.CurrentItem = player.inventory.Items[input];
        }
        private void ShowInventory()
        {
            state.Message = Data.Localize(Keys.Inventory, language);
            List<string> inventory = new List<string>();
            foreach (var item in player.inventory.Items)
            {
                if(item == player.CurrentClothes || item == player.CurrentWeapon)
                {
                    inventory.Add($"{ item.GetItemSpecs(language) } *{Data.Localize(Keys.Equiped, language)}*");
                }
                else
                {
                    inventory.Add(item.GetItemSpecs(language));
                }

            }
            ResetOptions(inventory);
            ProceedInput = InventoryDialog;
        }
    }
}