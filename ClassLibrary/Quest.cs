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
                { Keys.Search, Search },
                { Keys.Inventory, LaunchOpenInventoryDialog },
                { Keys.Cancel, Cancel },
                { Keys.Trade, LaunchTradeDialog },
                { Keys.Sell, LaunchSellDialog },
                { Keys.Buy, LaunchBuyDialog },
                { Keys.Eat, Eat },
                { Keys.Fight, Fight },
                { Keys.Run, Run },
                { Keys.NPC, LaunchNpcDialog },
                { Keys.Steal, LaunchStealDialog },
                { Keys.Loot, LaunchLootDialog },
               // { Keys.Fire, MakeABonfire },
                { Keys.Open, Open },
                { Keys.Poison, Poison },
                { Keys.Drink, CurePoison },
                { Keys.Trap, TriggerTrap },
                { Keys.HornetNest, TriggerHornetNest }


            };
            availableCommands = new List<Keys>();
            if (player.Name == "Test" || player.Name == "test")
            {
                questMap.SetPlayerLocation((0, 0));
            }
            else
            {
                questMap.SetPlayerLocation(((int)MainQuestConfig.MapSize / 2, (int)MainQuestConfig.MapSize / 2));
            }
            state.Message = $"Select preferred language:";
            availableCommands.AddRange(new List<Keys>() { Keys.EN, Keys.RU, Keys.UA });
            state.Options.AddRange(new List<string>() { "EN", "RU", "UA" });
            menuCallChain = new Stack<Action>();
            ProceedInput = NonInventoryDialogInputHandler;
            return state;
        }
        private void StartHandlingIntInput()
        {
            SetStateMessage(Keys.InitialMessage);
            if (questMap.PlayerSpot.npc != null && questMap.PlayerSpot.npc.IsHostile)
            {
                AppendStateMessage(Keys.Enemy);
            }
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
                Cancel();
            }
            else
            {
                SelectItemInPlayerInventory(input);
                ResetAvailableOptions(player.inventory.CurrentItem.Options);
                menuCallChain.Push(LaunchOpenInventoryDialog);
                ProceedInput = NonInventoryDialogInputHandler;
            }
            return state;
        }
        public QuestState SellDialogInputHandler(int input)
        {
            if (input == state.Options.Count - 1)
            {
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
                questMap.PlayerSpot.npc.IsHostile = true;
            }
            Cancel();
            return state;
        }
        public QuestState LootDialogInputHandler(int input)
        {
            if (input < state.Options.Count - 1)
            {
                StealOrLoot(input);
                LaunchLootDialog();
            }
            else
            {
                LaunchMainDialog();
            }
            return state;
        }
        private bool IsQuestEnded()
        {
            if(player.Health > 0 && !questMap.ExitReached)
            {
                return false;
            }
            else if(player.Health <= 0)
            {
                Loss();
            }
            else if (questMap.ExitReached)
            {
                EndQuest();
            }
            return true;
        }
        private void Fight()
        {
            while (player.Health > 0 && questMap.PlayerSpot.npc.Health > 0)
            {
                if (player.CurrentWeapon != null)
                {
                    questMap.PlayerSpot.npc.TakeHit(player.CurrentWeapon.Attack);
                }
                player.TakeHit(questMap.PlayerSpot.npc.Attack);
            }
            if (player.Health <= 0)
            {
                Loss();
            }
            else
            {
                LaunchNpcDialog();
            }
        }
        private void LaunchMainDialog()
        {
            AppendStateMessage(questMap.PlayerSpot.Description);
            if (!IsQuestEnded())
            {
                if (questMap.PlayerSpot.npc != null && questMap.PlayerSpot.npc.IsHostile)
                {
                    AppendStateMessage(Keys.Enemy, questMap.PlayerSpot.npc.Name);
                }
                state.PlayerStateOrAdditionalInformation = Data.StateBuilder(player, language);
                ResetAvailableOptions(questMap.PlayerSpot.GetListOfPossibleOptions());
                ResetMenuCallsChain();
                ProceedInput = NonInventoryDialogInputHandler;
            }
        }
        private void LaunchTravelDialog()
        {
            if (time.DayTime())
            {
                SetStateMessage(Keys.DirectionDialogMessage);
                ResetAvailableOptions(questMap.PlayerSpot.GetAvailableDirections());
            }
            else
            {
                SetStateMessage(Keys.NightTime);
                LaunchMainDialog();
            }
        }
        private void LaunchOpenInventoryDialog()
        {
            SetStateMessage(Keys.Inventory);
            ListInventoryForUseAndLoot(player.inventory);
            state.PlayerStateOrAdditionalInformation = Data.StateBuilder(player, language);
            ProceedInput = OpenInventoryDialogInputHandler;
        }
        private void LaunchNpcDialog()
        {
            SetStateMessage(questMap.PlayerSpot.npc.Name);
            ResetAvailableOptions(questMap.PlayerSpot.npc.GetListOfPossibleOptions());
            ProceedInput = NonInventoryDialogInputHandler;
        }
        private void LaunchTradeDialog()
        {
            SetStateMessage(questMap.PlayerSpot.npc.Name);
            ResetAvailableOptions(new List<Keys>() { Keys.Sell, Keys.Buy, Keys.Cancel });
            AddUniqueCallInMenueCallHistory(LaunchNpcDialog);
            ProceedInput = NonInventoryDialogInputHandler;
        }
        private void LaunchSellDialog()
        {
            AddUniqueCallInMenueCallHistory(LaunchTradeDialog);
            SetStateMessage(Keys.Sell);
            ListInventoryForTrading(player.inventory);
            state.PlayerStateOrAdditionalInformation = $"{ Data.Localize(Keys.Remains, language) } { player.inventory.Coins } { Data.Localize(Keys.Coins, language) }";
            ProceedInput = SellDialogInputHandler;
        }
        private void LaunchBuyDialog()
        {
            AddUniqueCallInMenueCallHistory(LaunchTradeDialog);
            SetStateMessage(Keys.Buy);
            ListInventoryForTrading(questMap.PlayerSpot.npc.inventory);
            state.PlayerStateOrAdditionalInformation = $"{ Data.Localize(Keys.Remains, language) } { player.inventory.Coins } { Data.Localize(Keys.Coins, language) }";
            ProceedInput = BuyDialogInputHandler;
        }
        private void LaunchStealDialog()
        {
            AddUniqueCallInMenueCallHistory(LaunchNpcDialog);
            SetStateMessage(questMap.PlayerSpot.npc.Name);
            AppendStateMessage(Keys.Steal);
            ListInventoryForUseAndLoot(questMap.PlayerSpot.npc.inventory);
            ProceedInput = StealDialogInputHandler;
        }
        private void LaunchLootDialog()
        {
            SetStateMessage(questMap.PlayerSpot.npc.Name);
            AppendStateMessage(Keys.Loot);
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
            AppendStateMessage(Keys.Death);
            EndQuest();
        }
        private void Sell(int input)
        {
            SelectItemInPlayerInventory(input);
            UnequipSelectedItem();
            questMap.PlayerSpot.npc.inventory.Add(player.inventory.CurrentItem);
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
            state.Message = null;
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
            if (time.DayTime())
            {
                SetStateMessage(Keys.DayTimeSleep);
            }
            else
            {
                time.ChangeTime(player.Sleep());
                SetStateMessage(Keys.WakeUp);
                state.PlayerStateOrAdditionalInformation = Data.StateBuilder(player, language);
            }
            LaunchMainDialog();
        }
        private void Rest()
        {
            time.ChangeTime(player.Rest());
            SetStateMessage(Keys.StaminaRecovered);
            LaunchMainDialog();
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
        private void PlayerReachedNewZone(int speedModifier = 1)
        {
            SetStateMessage(Keys.NextZone);
            time.ChangeTime(player.CalculateTimeNeededToTravel());
            player.RecaculateStateDueToTraveling(speedModifier);
            LaunchMainDialog();
        }
        private void Run()
        {
            questMap.Go(questMap.GetRandomAvailableDirection(questMap.PlayerSpot));
            PlayerReachedNewZone(2);
        }
        private void Search()
        {
            if (questMap.PlayerSpot.Description == Keys.Burrow && !questMap.PlayerSpot.searched)
            {
                if (player.CurrentWeapon != null)
                {
                    Hunt();
                }
                else
                {
                    SetStateMessage(Keys.HuntFailed);
                }
                questMap.PlayerSpot.searched = true;
            }
            else
            {
                SearchSpotForHiddenItem();
            }
        }
        private void Hunt()
        {
            SetStateMessage(Keys.HuntSucceed);
            questMap.PlayerSpot.npc = questMap.prefabs.hare;
            ListInventoryForUseAndLoot(questMap.PlayerSpot.npc.inventory);
            ProceedInput = LootDialogInputHandler;
        }
        private void SearchSpotForHiddenItem()
        {
            Item newItem = questMap.PlayerSpot.item;
            if (newItem != null)
            {
                if ((newItem is Consumable) && (newItem as Consumable).AutoConsume)
                {
                    commands[newItem.Use()].Invoke();
                }
                else
                {
                    player.inventory.Add(newItem);
                    questMap.PlayerSpot.item = null;
                }
                state.Message = null;
                AppendStateMessage(Keys.Found, newItem.Name);
            }
            else
            {
                SetStateMessage(Keys.NotFound);
            }
            time.ChangeTime(1);
            player.InnerStateProcess(1);
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
            questMap.PlayerSpot.npc.inventory.CurrentItem = player.inventory.CurrentItem;
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
            player.Eat(false);
            LaunchOpenInventoryDialog();
        }
        private void Poison()
        {
            SetStateMessage(Keys.PoisonedFood);
            player.Eat(true);
            LaunchOpenInventoryDialog();
        }
        private void TriggerTrap()
        {
            SetStateMessage(Keys.Trap);
            player.TakeHit(questMap.prefabs.trap.EffectPower);
            player.Effects.Add(Keys.Injure);
        }
        private void TriggerHornetNest()
        {
            SetStateMessage(Keys.HornetNest);
            player.TakeHit(questMap.prefabs.hornetNest.EffectPower);
            player.Effects.Add(Keys.Poison);
        }
        private void CurePoison()
        {
            SetStateMessage(Keys.CurePoison);
            LaunchOpenInventoryDialog();
        }
        private void Open()
        {
            player.inventory.AddMoney(player.inventory.CurrentItem.Price * 2);
            player.inventory.Drop();
            LaunchOpenInventoryDialog();
        }
        private void AddUniqueCallInMenueCallHistory(Action recentMenu)
        {
            if (menuCallChain.Peek() != recentMenu)
            {
                menuCallChain.Push(recentMenu);
            }
        }
        private void SetStateMessage(Keys key)
        {
            state.Message = $"{ Data.Localize(key, language) }";
        }
        private void AppendStateMessage(Keys key)
        {
            state.Message = $"{ state.Message }{ Environment.NewLine }{ Data.Localize(key, language) }";
        }
        private void AppendStateMessage(Keys firstKey, Keys secondKey)
        {
            state.Message = $"{ state.Message }{ Environment.NewLine }{ Data.Localize(firstKey, language) }{ Data.Localize(secondKey, language) }";
        }

    }
}