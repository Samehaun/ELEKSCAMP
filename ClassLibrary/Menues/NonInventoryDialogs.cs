using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ELEKSUNI
{
    abstract class NonInventoryDialogs
    {
        protected Report report;
        protected List<Keys> commands;
        protected List<Keys> newCommands;
        protected Player player;
        protected Spot currentSpot;
        protected Keys header;
        public NonInventoryDialogs(Quest quest)
        {
            report = quest.report;
            player = quest.player;
            commands = quest.availableCommands;
            currentSpot = quest.questMap.PlayerSpot;
        }
        public void Launch()
        {
            SetMessage();
            SetPlayerState();
            commands.Clear();
            SetAvailableOptions();
            SetOptionsView();
        }
        virtual protected void SetMessage()
        {
            report.SetReportMessage(header);
        }
        virtual protected void SetPlayerState()
        {
            report.RefreshPlayerState(player);
        }
        virtual protected void SetAvailableOptions()
        {
            commands.AddRange(newCommands);
        }
        virtual protected void SetOptionsView()
        {
            report.ResetOptions(commands);
        }
        protected List<Keys> GetListOfSameOptions(Keys option, int timesToRepeatOption)
        {
            List<Keys> repeatingList = Enumerable.Repeat(option, timesToRepeatOption).ToList();
            repeatingList.Add(Keys.Cancel);
            return repeatingList;
        }

    }
    class ItemDialog : NonInventoryDialogs
    {
        public ItemDialog(Quest quest) : base(quest) 
        { 
            header = player.GetActiveItem().Name;
            newCommands = player.GetActiveItem().Options;
        }
    }
    class TravelDialog : NonInventoryDialogs
    {       
        public TravelDialog(Quest quest) : base(quest)
        { 
            header = Keys.DirectionDialogMessage;
            newCommands = currentSpot.GetAvailableDirections();
        }
    }
    class MainDialog : NonInventoryDialogs
    {
        public MainDialog(Quest quest) : base(quest) 
        { 
            newCommands = currentSpot.GetListOfPossibleOptions();
        }
        protected override void SetMessage()
        {
            report.AddNewLineMessage(currentSpot.Description);
            if (currentSpot.npc != null && currentSpot.npc.IsHostile)
            {
                report.AddNewLineMessage(Keys.Enemy);
                report.AppendRepportMessage(currentSpot.npc.Name);
            }
        }
    }
    class NpcDialog : NonInventoryDialogs
    {
        public NpcDialog(Quest quest) : base(quest)
        {
            header = currentSpot.npc.Name;
            newCommands = currentSpot.npc.GetListOfPossibleOptions();
        }
    }
    class TradeDialog : NonInventoryDialogs
    {
        public TradeDialog(Quest quest) : base(quest)
        {
            newCommands = new List<Keys>() { Keys.Buy, Keys.Sell };
        }
        protected override void SetMessage()
        {
            report.AppendRepportMessage(Keys.Trade);
        }
    }
}
