using System.Collections.Generic;

namespace ELEKSUNI
{
    abstract class Command
    {
        protected Map map;
        protected Report report;
        protected InputHandler proceedInput;
        protected Quest quest;
        protected Time time;
        protected Player player;
        protected Commands commands;
        public Command(Quest quest)
        {
            this.report = quest.report;
            this.map = quest.questMap;
            proceedInput = quest.inputProcessor;
            this.quest = quest;
            this.time = quest.time;
            this.player = quest.player;
            this.commands = quest.commands;
        }
        abstract public void Execute();
    }
    class Commands
    {
        private Dictionary<Keys, Command> commands;
        public Commands(Quest quest)
        {
            commands = new Dictionary<Keys, Command>()
            {
                { Keys.North, new GoNorthCommand(quest) },
                { Keys.South, new GoSouthCommand(quest) },
                { Keys.East, new GoEastCommand(quest) },
                { Keys.West, new GoWestCommand(quest) },
                { Keys.Travel, new LaunchTravelDialogCommand(quest) },
                { Keys.Sleep, new SleepCommand(quest) },
                { Keys.Rest, new RestCommand(quest)},
                { Keys.EN, new SetEnglishCommand(quest)},
                { Keys.RU, new SetRussianCommand(quest)},
                { Keys.UA, new SetUkrainianCommand(quest)},
                { Keys.Drop, new DropCommand(quest) },
                { Keys.Equip, new EquipCommand(quest) },
                { Keys.Search, new SearchCommand(quest) },
                { Keys.Inventory, new LauncOpenInventoryDialogCommand(quest) },
                { Keys.Cancel, new CancelCommand(quest) },
                { Keys.Trade, new LaunchTradeDialogCommand(quest) },
                { Keys.Sell, new LaunchSellDialogCommand(quest) },
                { Keys.Buy, new LaunchBuyDialogCommand(quest) },
                { Keys.Eat, new EatCommand(quest) },
                { Keys.Fight, new FightCommand(quest) },
                { Keys.Run, new RunCommand(quest) },
                { Keys.NPC, new LaunchNpcDialogCommand(quest) },
                { Keys.Steal, new LaunchStealDialogCommand(quest) },
                { Keys.Loot, new LaunchLootDialogCommand(quest) },
                { Keys.Open, new OpenCommand(quest) },
                { Keys.EatPoison, new PoisonCommand(quest) },
                { Keys.Drink, new CurePoison(quest) },
                { Keys.Trap, new TriggerTrap(quest) },
                { Keys.HornetNest, new TriggerHornetNest(quest) }
            };
        }
        public Command GetCommand(Keys key)
        {
            return commands[key];
        }
        public static void MainDialog(Quest quest)
        {
            LaunchMainDialogCommand main = new LaunchMainDialogCommand(quest);
            main.Execute();
        }
        public static void NewZone(Quest quest, int speedModifier)
        {
            quest.report.SetReportMessage(Keys.NextZone);
            quest.time.ChangeTime(quest.player.CalculateTimeNeededToTravel());
            quest.player.RecaculateStateDueToTraveling(speedModifier);
            MainDialog(quest);
        }
        public List<Command> GetCommandList(List<Keys> keys)
        {
            List<Command> options = new List<Command>();
            foreach (var key in keys)
            {
                options.Add(commands[key]);
            }
            return options;
        }
        public static List<Command> GetListOfSelectionPlayerItemsCommand(Inventory inventory)
        {
            List<Command> options = new List<Command>();
            foreach (var item in inventory.Items)
            {
                options.Add(new SelectItemInPlayerInventoryCommand())
            }
        }
    }

    internal class TriggerHornetNest : Command
    {
        public TriggerHornetNest(Quest quest) : base(quest)
        {
        }
        public override void Execute()
        {
            throw new System.NotImplementedException();
        }
    }

    internal class TriggerTrap : Command
    {
        public TriggerTrap(Quest quest) : base(quest)
        {
        }
        public override void Execute()
        {
            throw new System.NotImplementedException();
        }
    }

    internal class CurePoison : Command
    {
        public CurePoison(Quest quest) : base(quest)
        {
        }
        public override void Execute()
        {
            throw new System.NotImplementedException();
        }
    }

    internal class PoisonCommand : Command
    {
        public PoisonCommand(Quest quest) : base(quest)
        {
        }
        public override void Execute()
        {
            throw new System.NotImplementedException();
        }
    }

    internal class OpenCommand : Command
    {
        public OpenCommand(Quest quest) : base(quest)
        {
        }
        public override void Execute()
        {
            throw new System.NotImplementedException();
        }
    }

    internal class LaunchLootDialogCommand : Command
    {
        public LaunchLootDialogCommand(Quest quest) : base(quest)
        {
        }
        public override void Execute()
        {
            throw new System.NotImplementedException();
        }
    }

    internal class LaunchStealDialogCommand : Command
    {
        public LaunchStealDialogCommand(Quest quest) : base(quest)
        {
        }
        public override void Execute()
        {
            throw new System.NotImplementedException();
        }
    }

    internal class LaunchNpcDialogCommand : Command
    {
        public LaunchNpcDialogCommand(Quest quest) : base(quest)
        {
        }
        public override void Execute()
        {
            throw new System.NotImplementedException();
        }
    }

    internal class RunCommand : Command
    {
        public RunCommand(Quest quest) : base(quest)
        {
        }
        public override void Execute()
        {
            throw new System.NotImplementedException();
        }
    }

    internal class FightCommand : Command
    {
        public FightCommand(Quest quest) : base(quest)
        {
        }
        public override void Execute()
        {
            throw new System.NotImplementedException();
        }
    }

    internal class EatCommand : Command
    {
        public EatCommand(Quest quest) : base(quest)
        {
        }
        public override void Execute()
        {
            throw new System.NotImplementedException();
        }
    }

    internal class LaunchBuyDialogCommand : Command
    {
        public LaunchBuyDialogCommand(Quest quest) : base(quest)
        {
        }
        public override void Execute()
        {
            throw new System.NotImplementedException();
        }
    }

    internal class LaunchSellDialogCommand : Command
    {
        public LaunchSellDialogCommand(Quest quest) : base(quest)
        {
        }
        public override void Execute()
        {
            throw new System.NotImplementedException();
        }
    }

    internal class LaunchTradeDialogCommand : Command
    {
        public LaunchTradeDialogCommand(Quest quest) : base(quest)
        {
        }
        public override void Execute()
        {
            throw new System.NotImplementedException();
        }
    }

    internal class LauncOpenInventoryDialogCommand : Command
    {
        public LauncOpenInventoryDialogCommand(Quest quest) : base(quest)
        {
        }
        public override void Execute()
        {
            throw new System.NotImplementedException();
        }
    }

    class SetEnglishCommand : Command
    {
        public SetEnglishCommand(Quest current) : base(current) { }
        public override void Execute()
        {
            quest.report.SetLanguage("EN");
            Commands.MainDialog(quest);
        }
    }
    class SetRussianCommand : Command
    {
        public SetRussianCommand(Quest current) : base(current) { }
        public override void Execute()
        {
            quest.report.SetLanguage("RU");
            Commands.MainDialog(quest);
        }
    }
    class SetUkrainianCommand : Command
    {
        public SetUkrainianCommand(Quest current) : base(current) { }
        public override void Execute()
        {
            quest.report.SetLanguage("UA");
            Commands.MainDialog(quest);
        }
    }
    class StartHandlingIntInputCommand : Command
    {
        public StartHandlingIntInputCommand(Quest quest) : base(quest) { }
        public override void Execute()
        {
            report.SetReportMessage(Keys.InitialMessage);
            if (map.PlayerSpot.npc != null && map.PlayerSpot.npc.IsHostile)
            {
                report.AddNewLineMessage(Keys.Enemy);
                report.AppendRepportMessage(map.PlayerSpot.npc.Name);
            }
            LaunchMainDialogCommand main = new LaunchMainDialogCommand(quest);
            main.Execute();
        }
    }
    class LaunchMainDialogCommand : Command
    {
        public LaunchMainDialogCommand(Quest quest) : base(quest) { }
        public override void Execute()
        {
            report.AddNewLineMessage(map.PlayerSpot.Description);
            if (!quest.IsQuestEnded())
            {
                if (map.PlayerSpot.npc != null && map.PlayerSpot.npc.IsHostile)
                {
                    report.AddNewLineMessage(Keys.Enemy);
                    report.AppendRepportMessage(map.PlayerSpot.npc.Name);
                }
                report.RefreshPlayerState(quest.player);
                report.ResetOptions(map.PlayerSpot.GetListOfPossibleOptions());
                proceedInput.SetCommands(commands.GetCommandList(map.PlayerSpot.GetListOfPossibleOptions()));
                proceedInput.ResetCommandsHistory(this);
            }
        }
    }
    class LaunchTravelDialogCommand : Command
    {
        public LaunchTravelDialogCommand(Quest quest) : base(quest) { }
        public override void Execute()
        {
            if (quest.time.DayTime())
            {
                report.SetReportMessage(Keys.DirectionDialogMessage);
                report.ResetOptions(map.PlayerSpot.GetAvailableDirections());
                proceedInput.SetCommands(commands.GetCommandList(map.PlayerSpot.GetListOfPossibleOptions()));
            }
            else
            {
                report.SetReportMessage(Keys.NightTime);
                LaunchMainDialogCommand main = new LaunchMainDialogCommand(quest);
                main.Execute();
            }
        }
    }
    class GoNorthCommand : Command
    {
        public GoNorthCommand(Quest quest) : base(quest) { }
        public override void Execute()
        {
            map.Go(Keys.North);
            Commands.NewZone(quest, 1);
        }
    }
    class GoSouthCommand : Command
    {
        public GoSouthCommand(Quest quest) : base(quest) { }
        public override void Execute()
        {
            map.Go(Keys.South);
            Commands.NewZone(quest, 1);
        }
    }
    class GoEastCommand : Command
    {
        public GoEastCommand(Quest quest) : base(quest) { }
        public override void Execute()
        {
            map.Go(Keys.East);
            Commands.NewZone(quest, 1);
        }
    }
    class GoWestCommand : Command
    {
        public GoWestCommand(Quest quest) : base(quest) { }
        public override void Execute()
        {
            map.Go(Keys.North);
            Commands.NewZone(quest, 1);
        }
    }
    class SleepCommand : Command
    {
        public SleepCommand(Quest quest) : base(quest) { }
        public override void Execute()
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
            Commands.MainDialog(quest);
        }
    }
    class RestCommand : Command
    {
        public RestCommand(Quest quest) : base(quest) { }
        public override void Execute()
        {
            time.ChangeTime(player.Rest());
            report.SetReportMessage(Keys.StaminaRecovered);
            Commands.MainDialog(quest);
        }
    }
    class DropCommand : Command
    {
        public DropCommand(Quest quest) : base(quest) { }
        public override void Execute()
        {
            quest.UnequipSelectedItem();
            player.Inventory.Drop();
            report.ClearReportMessage();
            proceedInput.PreviousMenu();
        }
    }
    class SelectItemInPlayerInventoryCommand : Command
    {
        int itemIndex;
        public SelectItemInPlayerInventoryCommand(Quest quest, int itemIndex) : base(quest)
        {
            this.itemIndex = itemIndex;
        }
        public override void Execute()
        {
            player.Inventory.CurrentItem = player.Inventory.Items[itemIndex];
        }
    }
    class SelectItemInNPCInventoryCommand : Command
    {
        int itemIndex;
        public SelectItemInNPCInventoryCommand(Quest quest, int itemIndex) : base(quest)
        {
            this.itemIndex = itemIndex;
        }
        public override void Execute()
        {
            player.Inventory.CurrentItem = map.PlayerSpot.npc.Inventory.Items[itemIndex];
        }
    }
    class EquipCommand : Command
    {
        public EquipCommand(Quest quest) : base(quest) { }
        public override void Execute()
        {
            if (player.Inventory.CurrentItem is Weapon)
            {
                player.CurrentWeapon = (Weapon)player.Inventory.CurrentItem;
            }
            else
            {
                player.CurrentClothes = (Clothes)player.Inventory.CurrentItem;
            }
            new CancelCommand(quest).Execute();
        }
    }
    class CancelCommand : Command
    {
        public CancelCommand(Quest quest) : base(quest) { }
        public override void Execute()
        {
            report.ClearReportMessage();
            proceedInput.PreviousMenu();
        }
    }
    class SearchCommand : Command
    {
        public SearchCommand(Quest quest) : base(quest) { }
        public override void Execute()
        {
            if (map.PlayerSpot.Description == Keys.Burrow && !map.PlayerSpot.searched)
            {
                if (player.CurrentWeapon != null)
                {
                    quest.Hunt();
                }
                else
                {
                    report.SetReportMessage(Keys.HuntFailed);
                }
                map.PlayerSpot.searched = true;
            }
            else
            {
                quest.SearchSpotForHiddenItem();
            }
        }
    }
}
