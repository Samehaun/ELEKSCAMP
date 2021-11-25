using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

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
        Dictionary<Keys, InputHandler> handlers;
        Time time;
        Map questMap;
        Player player;
        private List<Keys> availableCommands;
        public bool IsEnded { get; private set; }
        Report report;
        public delegate Report InputHandler(int input);
        public InputHandler ProceedInput;
        private Stack<Keys> menuCallChain;
        private Keys activeHandler;
        public Quest()
        {
            time = new Time();
            report = new Report();
            questMap = new Map();
            commands = new Dictionary<Keys, Action>()
            {
                { Keys.Main, LaunchMainDialog },
                { Keys.Item, LaunchItemDialog },
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
            handlers = new Dictionary<Keys, InputHandler>() 
            {
                { Keys.Inventory, OpenInventoryDialogInputHandler },
                { Keys.Sell, SellDialogInputHandler },
                { Keys.Buy, BuyDialogInputHandler },
                { Keys.Loot, LootDialogInputHandler },
                { Keys.Steal, StealDialogInputHandler },
                { Keys.Main, NonInventoryDialogInputHandler }
            };
            availableCommands = new List<Keys>();
            menuCallChain = new Stack<Keys>();
        }   
        public QuestState Save()
        {
            QuestState state = new QuestState();
            state.History = JsonConvert.SerializeObject(menuCallChain);
            state.Map = JsonConvert.SerializeObject(questMap.Save());
            state.Time = JsonConvert.SerializeObject(time.Save());
            state.Report = JsonConvert.SerializeObject(report.Save());
            state.ActiveHandler = JsonConvert.SerializeObject(activeHandler);
            state.Options = JsonConvert.SerializeObject(availableCommands);
            return state;
        }
        public void Load(QuestState save)
        {
            List<Keys> temp = JsonConvert.DeserializeObject<List<Keys>>(save.History);
            temp.Reverse();
            foreach (var key in temp)
            {
                menuCallChain.Push(key);
            }
            questMap.Load(JsonConvert.DeserializeObject<MapSave>(save.Map));
            player = questMap.player;
            time.Load(JsonConvert.DeserializeObject<TimeSave>(save.Time));
            AssignInputHandler(JsonConvert.DeserializeObject<Keys>(save.ActiveHandler));
            report.Load(JsonConvert.DeserializeObject<ReportSave>(save.Report));
            availableCommands = JsonConvert.DeserializeObject<List<Keys>>(save.Options);
        }
        private void AssignInputHandler(Keys key)
        {
            ProceedInput = handlers[key];
            activeHandler = key;
        }
        public Report Start(string name)
        {
            player = new Player(name);
            questMap = new Map(player);
            time = new Time();
            report = new Report();
            availableCommands = new List<Keys>();
            if (player.Name == "Test" || player.Name == "test")
            {
                questMap.SetPlayerLocation((0, 0));
            }
            else
            {
                questMap.SetPlayerLocation(((int)MainQuestConfig.MapSize / 2, (int)MainQuestConfig.MapSize / 2));
            }
            report.SetReportMessage($"Select preferred language:");
            availableCommands.AddRange(new List<Keys>() { Keys.EN, Keys.RU, Keys.UA });
            report.ResetOptions(new List<string>() { "EN", "RU", "UA" });
            menuCallChain = new Stack<Keys>();
            AssignInputHandler(Keys.Main);
            return report;
        }
        private void StartHandlingIntInput()
        {
            report.SetReportMessage(Keys.InitialMessage);
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
                LaunchItemDialog();
            }
            return report;
        }
        private void LaunchItemDialog()
        {
            ResetAvailableOptions(player.Inventory.CurrentItem.Options);
            AddUniqueCallInMenueCallHistory(Keys.Inventory);
            AssignInputHandler(Keys.Main);
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
                EndQuest(Keys.Death);
            }
            else if (questMap.ExitReached)
            {
                EndQuest(Keys.Exit);
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
                EndQuest(Keys.Death);
            }
            else
            {
                LaunchNpcDialog();
            }
        }
        private void LaunchMainDialog()
        {
            if (!IsQuestEnded())
            {
                report.AddNewLineMessage(questMap.PlayerSpot.Description);
                if (questMap.PlayerSpot.npc != null && questMap.PlayerSpot.npc.IsHostile)
                {
                    report.AddNewLineMessage(Keys.Enemy);
                    report.AppendRepportMessage(questMap.PlayerSpot.npc.Name);
                }
                report.RefreshPlayerState(player);
                ResetAvailableOptions(questMap.PlayerSpot.GetListOfPossibleOptions());
                ResetMenuCallsChain();
                AssignInputHandler(Keys.Main);
            }
        }
        private void LaunchTravelDialog()
        {
            if (time.DayTime())
            {
                report.SetReportMessage(Keys.DirectionDialogMessage);
                ResetAvailableOptions(questMap.PlayerSpot.GetAvailableDirections());
            }
            else
            {
                report.SetReportMessage(Keys.NightTime);
                LaunchMainDialog();
            }
        }
        private void LaunchOpenInventoryDialog()
        {
            report.SetReportMessage(Keys.Inventory);
            report.ShowInventoryForUseAndLoot(player.Inventory, player);
            AssignInputHandler(Keys.Inventory);
        }
        private void LaunchNpcDialog()
        {
            report.SetReportMessage(questMap.PlayerSpot.npc.Name);
            ResetAvailableOptions(questMap.PlayerSpot.npc.GetListOfPossibleOptions());
            AssignInputHandler(Keys.Main);
        }
        private void LaunchTradeDialog()
        {
            report.SetReportMessage(questMap.PlayerSpot.npc.Name);
            ResetAvailableOptions(new List<Keys>() { Keys.Sell, Keys.Buy, Keys.Cancel });
            AddUniqueCallInMenueCallHistory(Keys.NPC);
            AssignInputHandler(Keys.Main);
        }
        private void LaunchSellDialog()
        {
            AddUniqueCallInMenueCallHistory(Keys.Trade);
            report.SetReportMessage(Keys.Sell);
            report.ShowInventoryForTrading(player.Inventory, player);
            AssignInputHandler(Keys.Sell);
        }
        private void LaunchBuyDialog()
        {
            AddUniqueCallInMenueCallHistory(Keys.Trade);
            report.SetReportMessage(Keys.Buy);
            report.ShowInventoryForTrading(questMap.PlayerSpot.npc.inventory, player);
            AssignInputHandler(Keys.Buy);
        }
        private void LaunchStealDialog()
        {
            AddUniqueCallInMenueCallHistory(Keys.NPC);
            report.SetReportMessage(questMap.PlayerSpot.npc.Name);
            report.AddNewLineMessage(Keys.Steal);
            report.ShowInventoryForUseAndLoot(questMap.PlayerSpot.npc.inventory, player);
            AssignInputHandler(Keys.Steal);
        }
        private void LaunchLootDialog()
        {
            report.SetReportMessage(questMap.PlayerSpot.npc.Name);
            report.AddNewLineMessage(Keys.Loot);
            report.ShowInventoryForUseAndLoot(questMap.PlayerSpot.npc.inventory, player);
            AssignInputHandler(Keys.Loot);
        }
        private void EndQuest(Keys result)
        {
            IsEnded = true;
            report.EndingReport(result);
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
            report.ClearReportMessage();
            commands[menuCallChain.Pop()].Invoke();
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
            menuCallChain.Push(Keys.Main);
        }
        private void ResetAvailableOptions(List<Keys> options)
        {
            availableCommands.Clear();
            availableCommands.AddRange(options);
            report.ResetOptions(options);
        }
        private void SetEnglishAsQuestLanguage()
        {
            report.SetLanguage("EN");
            StartHandlingIntInput();
        }
        private void SetRussianAsQuestLanguage()
        {
            report.SetLanguage("RU");
            StartHandlingIntInput();
        }
        private void SetUkrainianAsQuestLanguage()
        {
            report.SetLanguage("UA");
            StartHandlingIntInput();
        }
        private void Sleep()
        {
            if (time.DayTime())
            {
                report.SetReportMessage(Keys.DayTimeSleep);
            }
            else
            {
                time.ChangeTime(player.Sleep());
                report.SetReportMessage(Keys.WakeUp);
                report.RefreshPlayerState(player);
            }
            LaunchMainDialog();
        }
        private void Rest()
        {
            time.ChangeTime(player.Rest());
            report.SetReportMessage(Keys.StaminaRecovered);
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
            report.SetReportMessage(Keys.NextZone);
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
                    report.SetReportMessage(Keys.HuntFailed);
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
            report.SetReportMessage(Keys.HuntSucceed);
            questMap.PlayerSpot.npc = questMap.prefabs.hare;
            report.ShowInventoryForUseAndLoot(questMap.PlayerSpot.npc.inventory, player);
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
                report.SetReportMessage(Keys.NotFound);
            }
            time.ChangeTime(1);
            player.InnerStateProcess(1);
            LaunchMainDialog();
        }
        private void FoundHiddenItem(Item newItem)
        {
            player.Inventory.Add(newItem);
            questMap.PlayerSpot.item = null;
            report.SetReportMessage(Keys.Found);
            report.AppendRepportMessage(newItem.Name);
        }
        private void SelectItemInPlayerInventory(int input)
        {
            player.Inventory.CurrentItem = player.Inventory.Items[input];
            report.SetReportMessage(player.Inventory.CurrentItem.Name);
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
            report.SetReportMessage(Keys.GetPoisonMessage);
            commands[menuCallChain.Pop()].Invoke();
        }
        private void TriggerTrap()
        {
            report.SetReportMessage(Keys.Trap);
            DisableTrigger(Keys.Bleeding);
        }
        private void TriggerHornetNest()
        {
            report.SetReportMessage(Keys.HornetNest);
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
            report.SetReportMessage(Keys.CurePoison);
            commands[menuCallChain.Pop()].Invoke();
        }
        private void Open()
        {
            player.Inventory.AddMoney(player.Inventory.CurrentItem.Price * 2);
            player.Inventory.Drop();
            LaunchOpenInventoryDialog();
        }
        private void AddUniqueCallInMenueCallHistory(Keys recentMenu)
        {
            if (menuCallChain.Peek() != recentMenu)
            {
                menuCallChain.Push(recentMenu);
            }
        }
    }

}