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
        Report report;
        public delegate Report InputHandler(int input);
        public InputHandler ProceedInput;
        private Stack<Action> menuCallChain;
        public Report Start(string name)
        {
            player = new Player(name);
            questMap = new Map(player);
            time = new Time();
            report = new Report();
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
                { Keys.Open, Open },
                { Keys.EatPoison, Poison },
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
            report.Message = $"Select preferred language:";
            availableCommands.AddRange(new List<Keys>() { Keys.EN, Keys.RU, Keys.UA });
            report.Options.AddRange(new List<string>() { "EN", "RU", "UA" });
            menuCallChain = new Stack<Action>();
            ProceedInput = NonInventoryDialogInputHandler;
            return report;
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
        public Report NonInventoryDialogInputHandler(int input)
        {
            commands[availableCommands[input]].Invoke();
            return report;
        }
        public Report OpenInventoryDialogInputHandler(int input)
        {
            if (input == report.Options.Count - 1)
            {
                Cancel();
            }
            else
            {
                SelectItemInPlayerInventory(input);
                ResetAvailableOptions(player.Inventory.CurrentItem.Options);
                AddUniqueCallInMenueCallHistory(LaunchOpenInventoryDialog);
                ProceedInput = NonInventoryDialogInputHandler;
            }
            return report;
        }
        public Report SellDialogInputHandler(int input)
        {
            if (input == report.Options.Count - 1)
            {
                Cancel();
            }
            else
            {
                Sell(input);
            }
            return report;
        }
        public Report BuyDialogInputHandler(int input)
        {
            if (input == report.Options.Count - 1)
            {
                Cancel();
            }
            else
            {
                TryPurchase(input);
            }
            return report;
        }
        public Report StealDialogInputHandler(int input)
        {
            if (input < report.Options.Count - 1)
            {
                StealOrLoot(input);
                questMap.PlayerSpot.npc.IsHostile = true;
            }
            Cancel();
            return report;
        }
        public Report LootDialogInputHandler(int input)
        {
            if (input < report.Options.Count - 1)
            {
                StealOrLoot(input);
                LaunchLootDialog();
            }
            else
            {
                LaunchMainDialog();
            }
            return report;
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
                report.PlayerStateOrAdditionalInformation = Data.PlayerStateBuilder(player, language);
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
            ListInventoryForUseAndLoot(player.Inventory);
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
            ListInventoryForTrading(player.Inventory);
            report.PlayerStateOrAdditionalInformation = $"{ Data.Localize(Keys.Remains, language) } { player.Inventory.Coins } { Data.Localize(Keys.Coins, language) }";
            ProceedInput = SellDialogInputHandler;
        }
        private void LaunchBuyDialog()
        {
            AddUniqueCallInMenueCallHistory(LaunchTradeDialog);
            SetStateMessage(Keys.Buy);
            ListInventoryForTrading(questMap.PlayerSpot.npc.inventory);
            report.PlayerStateOrAdditionalInformation = $"{ Data.Localize(Keys.Remains, language) } { player.Inventory.Coins } { Data.Localize(Keys.Coins, language) }";
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
            report.PlayerStateOrAdditionalInformation = null;
            report.Options = null;
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
            questMap.PlayerSpot.npc.inventory.Add(player.Inventory.CurrentItem);
            player.Inventory.Sell();
            LaunchSellDialog();
        }
        private void TryPurchase(int input)
        {
            SelectItemInNpcInventory(input);
            if (player.Inventory.Coins >= player.Inventory.CurrentItem.Price)
            {
                questMap.PlayerSpot.npc.inventory.Drop();
                player.Inventory.Buy();
                LaunchBuyDialog();
            }
        }
        private void Cancel()
        {
            report.Message = null;
            menuCallChain.Pop().Invoke();
        }
        private void StealOrLoot(int input)
        {
            SelectItemInNpcInventory(input);
            player.Inventory.Add(player.Inventory.CurrentItem);
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
            report.Options.Clear();
            report.Options.AddRange(Data.Localize(options, language));
        }
        private void ResetAvailableOptions(List<string> options)
        {
            report.Options.Clear();
            report.Options.AddRange(options);
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
                report.PlayerStateOrAdditionalInformation = Data.PlayerStateBuilder(player, language);
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
            if (player.Inventory.CurrentItem is Weapon)
            {
                player.CurrentWeapon = (Weapon)player.Inventory.CurrentItem;
            }
            else
            {
                player.CurrentClothes = (Clothes)player.Inventory.CurrentItem;
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
                    FoundHiddenItem(newItem);
                }
            }
            else
            {
                SetStateMessage(Keys.NotFound);
            }
            time.ChangeTime(1);
            player.InnerStateProcess(1);
            LaunchMainDialog();
        }

        private void FoundHiddenItem(Item newItem)
        {
            player.Inventory.Add(newItem);
            questMap.PlayerSpot.item = null;
            report.Message = null;
            AppendStateMessage(Keys.Found, newItem.Name);
        }

        private void SelectItemInPlayerInventory(int input)
        {
            player.Inventory.CurrentItem = player.Inventory.Items[input];
            report.PlayerStateOrAdditionalInformation = report.Options[input];
        }
        private void SelectItemInNpcInventory(int input)
        {
            player.Inventory.CurrentItem = questMap.PlayerSpot.npc.inventory.Items[input];
            questMap.PlayerSpot.npc.inventory.CurrentItem = player.Inventory.CurrentItem;
        }
        private void DropSelectedItem()
        {
            UnequipSelectedItem();
            player.Inventory.Drop();
            Cancel();
        }
        private void UnequipSelectedItem()
        {
            if (player.CurrentClothes == player.Inventory.CurrentItem)
            {
                player.CurrentClothes = null;
            }
            else if (player.CurrentWeapon == player.Inventory.CurrentItem)
            {
                player.CurrentWeapon = null;
            }
        }
        private void Eat()
        {
            player.Eat(false);
            Cancel();
        }
        private void Poison()
        {
            player.Eat(true);
            ComplexStateReport(Keys.GetPoisonMessage);
            Cancel();
        }
        private void TriggerTrap()
        {
            SetStateMessage(Keys.Trap);
            DisableTrigger(Keys.Bleeding);
        }
        private void TriggerHornetNest()
        {
            SetStateMessage(Keys.HornetNest);
            DisableTrigger(Keys.IsPoisoned);
        }
        private void DisableTrigger(Keys effect)
        {
            Consumable trigger = (Consumable)questMap.PlayerSpot.item;
            player.TakeHit(trigger.EffectPower);
            player.Effects.Add(effect);
            questMap.PlayerSpot.item = null;
        }
        private void CurePoison()
        {
            player.TakeAntidote();
            ComplexStateReport(Keys.CurePoison);
            Cancel();
        }
        private void Open()
        {
            player.Inventory.AddMoney(player.Inventory.CurrentItem.Price * 2);
            player.Inventory.Drop();
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
            report.Message = $"{ Data.Localize(key, language) }";
        }
        private void AppendStateMessage(Keys key)
        {
            report.Message = $"{ report.Message }{ Environment.NewLine }{ Data.Localize(key, language) }";
        }
        private void AppendStateMessage(Keys firstKey, Keys secondKey)
        {
            report.Message = $"{ report.Message }{ Environment.NewLine }{ Data.Localize(firstKey, language) }{ Data.Localize(secondKey, language) }";
        }
        private void ComplexStateReport(Keys key)
        {
            report.PlayerStateOrAdditionalInformation = $"{ Data.Localize(key, language) }{ Environment.NewLine }{ Data.PlayerStateBuilder(player, language) }";
        }
    }
}