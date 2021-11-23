using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        public Command(Quest quest)
        {
            this.report = quest.report;
            this.map = quest.questMap;
            proceedInput = quest.inputProcessor;
            this.quest = quest;
            this.time = quest.time;
            this.player = quest.player;
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
        }
        public void ExecuteCommand(Keys key)
        {
            commands[key].Execute();
        }
        public static void MainDialog(Quest quest)
        {
            LaunchMainDialogCommand main = new LaunchMainDialogCommand(quest);
            main.Execute();
        }
        public static void NewZone(Quest quest)
        {
            quest.report.SetReportMessage(Keys.NextZone);
            MainDialog(quest);
        }
        public static void UnequipSelectedItem(Player player)
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
            Commands.NewZone(quest);
        }
    }
    class GoSouthCommand : Command
    {
        public GoSouthCommand(Quest quest) : base(quest) { }
        public override void Execute()
        {
            map.Go(Keys.South);
            Commands.NewZone(quest);
        }
    }
    class GoEastCommand : Command
    {
        public GoEastCommand(Quest quest) : base(quest) { }
        public override void Execute()
        {
            map.Go(Keys.East);
            Commands.NewZone(quest);
        }
    }
    class GoWestCommand : Command
    {
        public GoWestCommand(Quest quest) : base(quest) { }
        public override void Execute()
        {
            map.Go(Keys.North);
            Commands.NewZone(quest);
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
            Commands.UnequipSelectedItem(player);
            player.Inventory.Drop();
            report.ClearReportMessage();
            proceedInput.PreviousMenu();
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
}
