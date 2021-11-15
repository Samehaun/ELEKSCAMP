using System;
using System.Collections.Generic;

namespace ELEKSUNI
{
    enum MainQuestConfig
    {
        BasePlayerSpeed = 5,
        BasePlayerStaminaConsuption = 5,
        MaxWeightPlayerCanCarry = 10,
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
        private Stack<Action> menuCallChain;
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
                { Keys.Travel, LaunchTravelDialog},
                { Keys.Sleep, Sleep},
                { Keys.Rest, Rest},
                { Keys.EN, SetEnglishAsQuestLanguage},
                { Keys.RU, SetRussianAsQuestLanguage},
                { Keys.UA, SetUkrainianAsQuestLanguage},
                { Keys.Drop, DropSelectedItem },
                { Keys.Equip, Equip },
                { Keys.Search, SearchSpotForHiddenItem },
                { Keys.Inventory, LaunchOpenInventoryDialog },
                { Keys.Cancel, Cancel },
                { Keys.Trade, LaunchTradeDialog },
                { Keys.Sell, LaunchSellDialog },
                { Keys.Buy, LaunchBuyDialog },
                { Keys.Eat, Eat },
                { Keys.Drink, Drink },
                { Keys.Fight, Fight },
                { Keys.Drink, Run }
            };
            availableCommands = new List<Keys>();
            questMap.SetPlayerLocation(((int)MainQuestConfig.MapSize / 2, (int)MainQuestConfig.MapSize / 2));
            state.Message = $"Select preferred language:";
            availableCommands.AddRange(new List<Keys>() { Keys.EN, Keys.RU, Keys.UA });
            state.Options.AddRange(new List<string>() { "EN", "RU", "UA" });
            ProceedInput = NonInventoryDialogInputHandler;
            return state;
        }
        private void StartHandlingIntInput()
        {
            string initialMessage = Data.Localize(Keys.InitialMessage, language);
            state.Message = $"{ initialMessage } {Environment.NewLine} { Data.Localize(questMap.GetLocationDescription(), language) }";
            menuCallChain = new Stack<Action>();
            LaunchMainDialog();
        }
        public QuestState NonInventoryDialogInputHandler(int input)
        {
            commands[availableCommands[input]].Invoke();
            return state;
        }
        public QuestState OpenInventoryDialogInputHandler(int input)
        {
            if (input == state.Options.Count - 1)
            {
                state.Message = Data.Localize(Keys.Trade, language);
                Cancel();
            }
            else
            {
                SelectItemInPlayerInventory(input);
                ResetAvailableOptions(new List<Keys>() { { player.inventory.CurrentItem.Use }, { Keys.Drop }, { Keys.Cancel } });
                menuCallChain.Push(LaunchOpenInventoryDialog);
                ProceedInput = NonInventoryDialogInputHandler;
            }
            return state;
        }
        public QuestState SellDialogInputHandler(int input)
        {
            if (input == state.Options.Count - 1)
            {
                state.Message = Data.Localize(Keys.Trade, language);
                Cancel();
            }
            else
            {
                Sell(input);
            }
            return state;
        }
        public QuestState BuyDialogInputHandler(int input)
        {
            if (input == state.Options.Count - 1)
            {
                state.Message = Data.Localize(Keys.Trade, language);
                Cancel();
            }
            else
            {
                TryPurchase(input);
            }
            return state;
        }
        public QuestState StealDialogInputHandler(int input)
        {
            if (input < state.Options.Count - 1)
            {
                StealOrLoot(input);
            }
            Cancel();
            return state;
        }
        public QuestState LootDialogInputHandler(int input)
        {
            if (input < state.Options.Count - 1)
            {
                StealOrLoot(input);
            }
            else
            {
                LaunchMainDialog();
            }
            return state;
        }
        private void LaunchMainDialog()
        {
            if (!questMap.ExitReached && player.Health > 0)
            {
                state.PlayerStateOrAdditionalInformation = Data.StateBuilder(player, language);
                ResetAvailableOptions(questMap.GetPossibleOptions());
            }
            else if (player.Health <= 0)
            {
                Loss();
            }
            else
            {
                EndQuest();
            }
            ResetMenuCallsChain();
            ProceedInput = NonInventoryDialogInputHandler;
        }
        private void LaunchTravelDialog()
        {
            if (time.NotNightTime())
            {
                state.Message = Data.Localize(Keys.DirectionDialogMessage, language);
                ResetAvailableOptions(questMap.GetTravelDirections());
            }
            else
            {
                state.Message = $"{ Data.Localize(Keys.NightTime, language) } { Environment.NewLine } { Data.Localize(questMap.GetLocationDescription(), language) }";
            }
        }
        private void LaunchOpenInventoryDialog()
        {
            state.Message = Data.Localize(Keys.Inventory, language);
            ListInventoryForUseAndLoot(player.inventory);
            state.PlayerStateOrAdditionalInformation = Data.StateBuilder(player, language);
            ProceedInput = OpenInventoryDialogInputHandler;
        }
        private void LaunchNpcDialog()
        {
            state.Message = Data.Localize(questMap.PlayerSpot.npc.Name, language);
            ResetAvailableOptions(questMap.PlayerSpot.npc.GetListOfPossibleOptions());
        }
        private void LaunchTradeDialog()
        {
            state.Message = Data.Localize(questMap.PlayerSpot.npc.Name, language);
            ResetAvailableOptions(new List<Keys>() { Keys.Sell, Keys.Buy, Keys.Cancel });
            menuCallChain.Push(LaunchNpcDialog);
        }
        private void LaunchSellDialog()
        {
            menuCallChain.Push(LaunchTradeDialog);
            state.Message = Data.Localize(Keys.Sell, language);
            ListInventoryForTrading(player.inventory);
            state.PlayerStateOrAdditionalInformation = $"{ Data.Localize(Keys.Remains, language) } { player.inventory.Coins } { Data.Localize(Keys.Coins, language) }";
            ProceedInput = SellDialogInputHandler;
        }
        private void LaunchBuyDialog()
        {
            menuCallChain.Push(LaunchTradeDialog);
            state.Message = Data.Localize(Keys.Buy, language);
            ListInventoryForTrading(questMap.PlayerSpot.npc.inventory);
            state.PlayerStateOrAdditionalInformation = $"{ Data.Localize(Keys.Remains, language) } { player.inventory.Coins } { Data.Localize(Keys.Coins, language) }";
            ProceedInput = BuyDialogInputHandler;
        }
        private void LaunchStealDialog()
        {
            menuCallChain.Push(LaunchNpcDialog);
            state.Message = Data.Localize(questMap.PlayerSpot.npc.Name, language);
            state.PlayerStateOrAdditionalInformation = Data.Localize(Keys.Steal, language);
            ListInventoryForUseAndLoot(questMap.PlayerSpot.npc.inventory);
            ProceedInput = StealDialogInputHandler;
        }
        private void LaunchLootDialog()
        {
            state.Message = Data.Localize(questMap.PlayerSpot.npc.Name, language);
            state.PlayerStateOrAdditionalInformation = Data.Localize(Keys.Loot, language);
            ListInventoryForUseAndLoot(questMap.PlayerSpot.npc.inventory);
            ProceedInput = LootDialogInputHandler;
        }
        private void ListInventoryForUseAndLoot(Inventory inventory)
        {
            List<string> itemsDescriptions = new List<string>();
            foreach (var item in inventory.Items)
            {
                if (item == player.CurrentClothes || item == player.CurrentWeapon)
                {
                    itemsDescriptions.Add($"{ item.GetItemSpecs(language) } *{Data.Localize(Keys.Equiped, language)}*");
                }
                else
                {
                    itemsDescriptions.Add(item.GetItemSpecs(language));
                }
            }
            itemsDescriptions.Add(Data.Localize(Keys.Cancel, language));
            ResetAvailableOptions(itemsDescriptions);
        }
        private void ListInventoryForTrading(Inventory inventory)
        {
            List<string> itemsDescriptions = new List<string>();
            foreach (var item in inventory.Items)
            {
                if (item == player.CurrentClothes || item == player.CurrentWeapon)
                {
                    itemsDescriptions.Add($"{ item.GetItemSpecsForTrade(language) } *{Data.Localize(Keys.Equiped, language)}*");
                }
                else
                {
                    itemsDescriptions.Add(item.GetItemSpecsForTrade(language));
                }
            }
            itemsDescriptions.Add(Data.Localize(Keys.Cancel, language));
            ResetAvailableOptions(itemsDescriptions);
        }
        private void EndQuest()
        {
            IsEnded = true;
            state.PlayerStateOrAdditionalInformation = null;
            state.Options = null;
        }
        private void Loss()
        {
            state.Message = Data.Localize(Keys.Death, language);
            EndQuest();
        }
        private void Sell(int input)
        {
            SelectItemInPlayerInventory(input);
            UnequipSelectedItem();
            player.inventory.Sell();
            LaunchSellDialog();
        }
        private void TryPurchase(int input)
        {
            SelectItemInNpcInventory(input);
            if (player.inventory.Coins >= player.inventory.CurrentItem.Price)
            {
                questMap.PlayerSpot.npc.inventory.Drop();
                player.inventory.Buy();
                LaunchBuyDialog();
            }
        }
        private void Cancel()
        {
            menuCallChain.Pop().Invoke();
        }
        private void StealOrLoot(int input)
        {
            SelectItemInNpcInventory(input);
            player.inventory.Add(player.inventory.CurrentItem);
            questMap.PlayerSpot.npc.inventory.Drop();
        }
        private void ResetMenuCallsChain()
        {
            menuCallChain.Clear();
            menuCallChain.Push(LaunchMainDialog);
        }
        private void ResetAvailableOptions(List<Keys> options)
        {
            availableCommands.Clear();
            availableCommands.AddRange(options);
            state.Options.Clear();
            state.Options.AddRange(Data.Localize(options, language));
        }
        private void ResetAvailableOptions(List<string> options)
        {
            state.Options.Clear();
            state.Options.AddRange(options);
        }
        private void SetEnglishAsQuestLanguage()
        {
            language = "EN";
            StartHandlingIntInput();
        }
        private void SetRussianAsQuestLanguage()
        {
            language = "RU";
            StartHandlingIntInput();
        }
        private void SetUkrainianAsQuestLanguage()
        {
            language = "UA";
            StartHandlingIntInput();
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
                state.PlayerStateOrAdditionalInformation = Data.StateBuilder(player, language);
            }
        }
        private void Rest()
        {
            time.ChangeTime(player.Rest());
            state.Message = $"{Data.Localize(Keys.StaminaRecovered, language)} {Environment.NewLine} { Data.Localize(questMap.GetLocationDescription(), language) }";
            state.PlayerStateOrAdditionalInformation = Data.StateBuilder(player, language);
        }
        private void GoNorth()
        {
            questMap.Go(Keys.North);
            PlayerReachedNewZone();
        }
        private void GoSouth()
        {
            questMap.Go(Keys.South);
            PlayerReachedNewZone();
        }
        private void GoEast()
        {
            questMap.Go(Keys.East);
            PlayerReachedNewZone();
        }
        private void GoWest()
        {
            questMap.Go(Keys.West);
            PlayerReachedNewZone();
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
            Cancel();
        }
        private void PlayerReachedNewZone()
        {
            state.Message = $"{ Data.Localize(Keys.NextZone, language) } { Environment.NewLine } { Data.Localize(questMap.GetLocationDescription(), language) }";
            time.ChangeTime(player.CalculateTimeNeededToTravel());
            player.RecaculateStateDueToTraveling();
            LaunchMainDialog();
        }
        private void SearchSpotForHiddenItem()
        {
            Item newItem = questMap.PlayerSpot.item;
            if (newItem != null)
            {
                player.inventory.Add(newItem);
                questMap.PlayerSpot.item = null;
                state.Message = $"{ Data.Localize(Keys.Found, language) } {Data.Localize(Keys.SimpleClothes, language)}";
            }
            else
            {
                state.Message = $" {Data.Localize(Keys.NotFound, language)}";
            }
            time.ChangeTime(1);
            player.RecaculateStateDueToTraveling();
            LaunchMainDialog();
        }
        private void SelectItemInPlayerInventory(int input)
        {
            player.inventory.CurrentItem = player.inventory.Items[input];
            state.PlayerStateOrAdditionalInformation = state.Options[input];
        }
        private void SelectItemInNpcInventory(int input)
        {
            player.inventory.CurrentItem = questMap.PlayerSpot.npc.inventory.Items[input];
            questMap.PlayerSpot.npc.inventory.CurrentItem = questMap.PlayerSpot.npc.inventory.Items[input];
        }
        private void DropSelectedItem()
        {
            UnequipSelectedItem();
            player.inventory.Drop();
            Cancel();
        }
        private void UnequipSelectedItem()
        {
            if (player.CurrentClothes == player.inventory.CurrentItem)
            {
                player.CurrentClothes = null;
            }
            else if (player.CurrentWeapon == player.inventory.CurrentItem)
            {
                player.CurrentWeapon = null;
            }
        }
        private void Eat()
        {
            Cancel();
        }
        private void Drink()
        {
            Cancel();
        }

    }
}