using System;
using System.Collections.Generic;
using Newtonsoft.Json;

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
        internal Map questMap;
        internal Player player;
        Prefabs prefabs;
        internal List<Keys> availableCommands;
        public bool IsEnded { get; private set; }
        internal Report report;
        private Stack<Keys> menuCallChain;
        private MainDialog mainDialog;
        private ItemDialog itemDialog;
        private TravelDialog travelDialog;
        private TradeDialog tradeDialog;
        private BuyDialog buyDialog;
        private SellDialog sellDialog;
        private StealDialog stealDialog;
        public Quest()
        {
            time = new Time();
            report = new Report();
            prefabs = new Prefabs();
            questMap = new Map(prefabs);
            player = new Player();
            menuCallChain = new Stack<Keys>();           
            availableCommands = new List<Keys>();
        }
        private void MapCommands()
        {
            mainDialog = new MainDialog(this);
            itemDialog = new ItemDialog(this);
            travelDialog = new TravelDialog(this);
            tradeDialog = new TradeDialog(this);
            buyDialog = new BuyDialog(this);
            sellDialog = new SellDialog(this);
            stealDialog = new StealDialog(this);
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
                { Keys.Inventory, new OpenInventoryDialog(this).Launch },
                { Keys.Cancel, Cancel },
                { Keys.Trade, LaunchTradeDialog },
                { Keys.Sell, LaunchSellDialog },
                { Keys.Buy, LaunchBuyDialog },
                { Keys.Eat, Eat },
                { Keys.Fight, Fight },
                { Keys.Run, Run },
                { Keys.NPC, new NpcDialog(this).Launch },
                { Keys.Steal, LaunchStealDialog },
                { Keys.Loot, new LootDialog(this).Launch },
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
            MapCommands();
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
            MapCommands();
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
            player.SelectItemToOperate(index);
            itemDialog.Launch();
        }
        private void Steal(int index)
        {
            player.Steal(index, questMap.PlayerSpot.npc);
            commands[Keys.NPC].Invoke();
        }
        private void Loot(int index)
        {
            player.Take(index, questMap.PlayerSpot.npc);
            commands[Keys.Loot].Invoke();
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
                commands[Keys.NPC].Invoke();
            }
        }
        private void LaunchItemDialog()
        {
            itemDialog.Launch();
            AddUniqueCallInMenueCallHistory(Keys.Inventory);
        }
        private void LaunchMainDialog()
        {
            if (!IsQuestEnded())
            {
                mainDialog.Launch();
                ResetMenuCallsChain();
            }
        }
        private void LaunchTravelDialog()
        {
            if (time.DayTime())
            {
                travelDialog.Launch();
            }
            else
            {
                report.SetReportMessage(Keys.NightTime);
                mainDialog.Launch();
            }
        }
        private void LaunchTradeDialog()
        {
            tradeDialog.Launch();
            AddUniqueCallInMenueCallHistory(Keys.NPC);
        }
        private void LaunchSellDialog()
        {
            AddUniqueCallInMenueCallHistory(Keys.Trade);
            sellDialog.Launch();
        }
        private void LaunchBuyDialog()
        {
            AddUniqueCallInMenueCallHistory(Keys.Trade);
            buyDialog.Launch();
        }
        private void LaunchStealDialog()
        {
            AddUniqueCallInMenueCallHistory(Keys.NPC);
            stealDialog.Launch();
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
        private void ResetMenuCallsChain()
        {
            menuCallChain.Clear();
            menuCallChain.Push(Keys.Main);
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
            commands[Keys.Loot].Invoke();
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
        private void Eat()
        {
            player.Eat(false);
            Cancel();
        }
        private void Poison()
        {
            player.Eat(true);
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
            player.AddEffect(effect);
            questMap.PlayerSpot.item = null;
        }
        private void CurePoison()
        {
            player.TakeAntidote();
            commands[menuCallChain.Pop()].Invoke();
        }
        private void Open()
        {
            player.Open();
            commands[Keys.Inventory].Invoke();
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