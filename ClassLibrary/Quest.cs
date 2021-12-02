using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;

namespace ELEKSUNI
{
    enum MainQuestConfig
    {
        BasePlayerSpeed = 5,
        BasePlayerStaminaConsuption = 5,
        MaxWeightPlayerCanCarry = 7,
        BaseTimeToChangeLocation = 3,
        MapSize = 4,
        MazeDifficulty = 2
    }
    public class Quest
    {
        Dictionary<Keys, Action> commands;
        Dictionary<Keys, Action<int>> commandsWithArgs;
        Time time;
        Map questMap;
        Player player;
        Prefabs prefabs;
        private List<Keys> availableCommands;
        public bool IsEnded { get; private set; }
        Report report;
        private Stack<Keys> menuCallChain;
        public Quest()
        {
            time = new Time();
            report = new Report();
            prefabs = new Prefabs();
            questMap = new Map(prefabs);
            player = new Player();
            commands = new Dictionary<Keys, Action>()
            {
                { Keys.Main, LaunchMainDialog },
                { Keys.Item, LaunchItemDialog },
                { Keys.North, () => Go(Keys.North) },
                { Keys.South, () => Go(Keys.South) },
                { Keys.East, () => Go(Keys.East) },
                { Keys.West, () => Go(Keys.West) },
                { Keys.Travel, LaunchTravelDialog},
                { Keys.Sleep, Sleep},
                { Keys.Rest, Rest},
                { Keys.EN, () => SetQuestLanguage("EN")},
                { Keys.RU, () => SetQuestLanguage("EN")},
                { Keys.UA, () => SetQuestLanguage("EN")},
                { Keys.Drop, player.Drop },
                { Keys.Equip, player.Equip },
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
                { Keys.Trap, () => TriggerTrap(Keys.Trap, Keys.Bleeding) },
                { Keys.HornetNest, () => TriggerTrap(Keys.HornetNest, Keys.IsPoisoned) }
            };
            commandsWithArgs = new Dictionary<Keys, Action<int>>()
            {
                { Keys.SellItem, Sell },
                { Keys.BuyItem, TryPurchase },
                { Keys.Select, SelectItemToOperate },
                { Keys.StealItem, Steal },
                { Keys.LootItem, Loot }
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
            state.Options = JsonConvert.SerializeObject(availableCommands);
            state.Player = JsonConvert.SerializeObject(player.Save());
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
            player.Load(JsonConvert.DeserializeObject<PlayerSave>(save.Player), prefabs);
            time.Load(JsonConvert.DeserializeObject<TimeSave>(save.Time));
            report.Load(JsonConvert.DeserializeObject<ReportSave>(save.Report));
            availableCommands = JsonConvert.DeserializeObject<List<Keys>>(save.Options);
        }
        public Report Start(string name)
        {
            player = new Player(name);
            player.PickItem(prefabs.simpleClothes);
            player.CurrentClothes = prefabs.simpleClothes;
            availableCommands = new List<Keys>();
            if (player.Name == "Test" || player.Name == "test")
            {
                questMap.CreateTestMap();
                questMap.SetPlayerLocation((0, 0));
            }
            else
            {
                questMap.CreateNewMap((int)MainQuestConfig.MapSize);
                questMap.SetPlayerLocation(((int)MainQuestConfig.MapSize / 2, (int)MainQuestConfig.MapSize / 2));
            }
            report.SetReportMessage($"Select preferred language:");
            availableCommands.AddRange(new List<Keys>() { Keys.EN, Keys.RU, Keys.UA });
            report.ResetOptions(new List<string>() { "EN", "RU", "UA" });
            menuCallChain = new Stack<Keys>();
            return report;
        }
        public Report ProceedInput(int input)
        {
            if (IsSelectedActionNeedsIndexParam(input))
            {
                commandsWithArgs[availableCommands[input]].Invoke(input);
            }
            else
            {
                commands[availableCommands[input]].Invoke();
            }
            return report;
        }
        private void SelectItemToOperate(int index)
        {
            SelectItemInPlayerInventory(index);
            LaunchItemDialog();
        }
        private void Steal(int index)
        {
            TakeItemFromNpcInventory(index);
            questMap.PlayerSpot.npc.IsHostile = true;
            LaunchNpcDialog();
        }
        private void Loot(int index)
        {
            TakeItemFromNpcInventory(index);
            LaunchLootDialog();
        }
        private bool IsQuestEnded()
        {
            if (player.Health > 0 && !questMap.ExitReached)
            {
                return false;
            }
            else if (player.Health <= 0)
            {
                EndQuest(Keys.Death);
            }
            else if (questMap.ExitReached)
            {
                EndQuest(Keys.Exit);
            }
            return true;
        }
        private bool IsSelectedActionNeedsIndexParam(int index)
        {
            if (availableCommands[index] == Keys.SellItem || availableCommands[index] == Keys.BuyItem
                || availableCommands[index] == Keys.StealItem || availableCommands[index] == Keys.LootItem
                || availableCommands[index] == Keys.Select)
            {
                return true;
            }
            else
            {
                return false;
            }
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
        private void LaunchItemDialog()
        {
            ResetAvailableOptions(player.GetActiveItem().Options);
            AddUniqueCallInMenueCallHistory(Keys.Inventory);
        }
        private void LaunchNpcDialog()
        {
            report.SetReportMessage(questMap.PlayerSpot.npc.Name);
            ResetAvailableOptions(questMap.PlayerSpot.npc.GetListOfPossibleOptions());
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
        private void LaunchTradeDialog()
        {
            report.SetReportMessage(questMap.PlayerSpot.npc.Name);
            ResetAvailableOptions(new List<Keys>() { Keys.Sell, Keys.Buy, Keys.Cancel });
            AddUniqueCallInMenueCallHistory(Keys.NPC);
        }
        private void LaunchOpenInventoryDialog()
        {
            ResetAvailableOptions(Enumerable.Repeat(Keys.Select, player.GetListOfItemsInInventory().Count).ToList(), false);
            availableCommands.Add(Keys.Cancel);
            report.SetReportMessage(Keys.Inventory);
            report.ShowInventory(player.GetListOfItemsInInventory(), player, report.ItemSpecs);
        }
        private void LaunchSellDialog()
        {
            AddUniqueCallInMenueCallHistory(Keys.Trade);
            ResetAvailableOptions(Enumerable.Repeat(Keys.SellItem, player.GetListOfItemsInInventory().Count).ToList(), false);
            availableCommands.Add(Keys.Cancel);
            report.SetReportMessage(Keys.Sell);
            report.ShowInventory(player.GetListOfItemsInInventory(), player, report.ItemSpecsForTrading);
        }
        private void LaunchBuyDialog()
        {
            AddUniqueCallInMenueCallHistory(Keys.Trade);
            ResetAvailableOptions(Enumerable.Repeat(Keys.BuyItem, questMap.PlayerSpot.npc.GetListOfItemsInInventory().Count).ToList(), false);
            availableCommands.Add(Keys.Cancel);
            report.SetReportMessage(Keys.Buy);
            report.ShowInventory(questMap.PlayerSpot.npc.GetListOfItemsInInventory(), player, report.ItemSpecsForTrading);
        }
        private void LaunchStealDialog()
        {
            AddUniqueCallInMenueCallHistory(Keys.NPC);
            ResetAvailableOptions(Enumerable.Repeat(Keys.StealItem, questMap.PlayerSpot.npc.GetListOfItemsInInventory().Count).ToList(), false);
            report.SetReportMessage(questMap.PlayerSpot.npc.Name);
            report.AddNewLineMessage(Keys.Steal);
            availableCommands.Add(Keys.Cancel);
            report.ShowInventory(questMap.PlayerSpot.npc.GetListOfItemsInInventory(), player, report.ItemSpecs);
        }
        private void LaunchLootDialog()
        {
            ResetAvailableOptions(Enumerable.Repeat(Keys.LootItem, questMap.PlayerSpot.npc.inventory.Items.Count).ToList(), false);
            report.SetReportMessage(questMap.PlayerSpot.npc.Name);
            report.AddNewLineMessage(Keys.Loot);
            availableCommands.Add(Keys.Main);
            report.ShowInventory(questMap.PlayerSpot.npc.GetListOfItemsInInventory(), player, report.ItemSpecs);
        }
        private void EndQuest(Keys result)
        {
            IsEnded = true;
            report.EndingReport(result);
        }
        private void Sell(int input)
        {
            player.Sell(input, questMap.PlayerSpot.npc);
            LaunchSellDialog();
        }
        private void TryPurchase(int input)
        {
            player.Buy(input, questMap.PlayerSpot.npc);
            LaunchBuyDialog();
        }
        private void Cancel()
        {
            report.ClearReportMessage();
            commands[menuCallChain.Pop()].Invoke();
        }
        private void TakeItemFromNpcInventory(int input)
        {
            player.Take(input, questMap.PlayerSpot.npc);
        }
        private void ResetMenuCallsChain()
        {
            menuCallChain.Clear();
            menuCallChain.Push(Keys.Main);
        }
        private void ResetAvailableOptions(List<Keys> options, bool useCommandNameAsDisplayOption = true)
        {
            availableCommands.Clear();
            availableCommands.AddRange(options);
            if (useCommandNameAsDisplayOption)
            {
                report.ResetOptions(options);
            }
        }
        private void SetQuestLanguage(string language)
        {
            report.SetLanguage(language);
            LaunchMainDialog();
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
        private void Go(Keys direction)
        {
            questMap.Go(direction);
            PlayerReachedNewZone();
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
            LaunchLootDialog();
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
            player.PickItem(newItem);
            questMap.PlayerSpot.item = null;
            report.SetReportMessage(Keys.Found);
            report.AppendRepportMessage(newItem.Name);
        }
        private void SelectItemInPlayerInventory(int input)
        {
            report.SetReportMessage(player.SelectItemToOperate(input));
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
        private void TriggerTrap(Keys trapName, Keys effect)
        {
            report.SetReportMessage(trapName);
            DisableTrigger(effect);
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