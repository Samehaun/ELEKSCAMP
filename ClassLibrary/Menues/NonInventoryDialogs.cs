using System.Collections.Generic;

namespace ELEKSUNI
{
    abstract class NonInventoryDialogs
    {
        protected Report report;
        protected List<Keys> commands;
        protected Player player;
        protected Map map;
        public NonInventoryDialogs(Quest quest)
        {
            report = quest.report;
            player = quest.player;
            commands = quest.availableCommands;
            map = quest.questMap;
        }
        public void Launch()
        {
            SetMessage();
            SetPlayerState();
            commands.Clear();
            SetAvailableOptions();
            SetOptionsView();
        }
        abstract protected void SetMessage();
        virtual protected void SetPlayerState()
        {
            report.RefreshPlayerState(player);
        }
        abstract protected void SetAvailableOptions();
        virtual protected void SetOptionsView()
        {
            report.ResetOptions(commands);
        }
    }
    class ItemDialog : NonInventoryDialogs
    {
        public ItemDialog(Quest quest) : base(quest)  {  }
        protected override void SetAvailableOptions()
        {
            commands.AddRange(player.GetActiveItem().Options);
        }
        protected override void SetMessage()
        {
            report.SetReportMessage(player.GetActiveItem().Name);
        }
    }
    class TravelDialog : NonInventoryDialogs
    {       
        public TravelDialog(Quest quest) : base(quest) {  }
        protected override void SetAvailableOptions()
        {
            commands.AddRange(map.PlayerSpot.GetAvailableDirections());
        }
        protected override void SetMessage()
        {
            report.SetReportMessage(Keys.DirectionDialogMessage);
        }
    }
    class MainDialog : NonInventoryDialogs
    {
        public MainDialog(Quest quest) : base(quest)  {  }
        protected override void SetMessage()
        {
            report.AddNewLineMessage(map.PlayerSpot.Description);
            if (map.PlayerSpot.npc != null && map.PlayerSpot.npc.IsHostile)
            {
                report.AddNewLineMessage(Keys.Enemy);
                report.AppendRepportMessage(map.PlayerSpot.npc.Name);
            }
        }
        protected override void SetAvailableOptions()
        {
            commands.AddRange(map.PlayerSpot.GetListOfPossibleOptions());
        }
    }
    class NpcDialog : NonInventoryDialogs
    {
        public NpcDialog(Quest quest) : base(quest) { }
        protected override void SetAvailableOptions()
        {
            commands.AddRange(map.PlayerSpot.npc.GetListOfPossibleOptions());
        }
        protected override void SetMessage()
        {
            report.SetReportMessage(map.PlayerSpot.npc.Name);
        }
    }
    class TradeDialog : NonInventoryDialogs
    {
        public TradeDialog(Quest quest) : base(quest) { }
        protected override void SetMessage()
        {
            report.AppendRepportMessage(Keys.Trade);
        }
        protected override void SetAvailableOptions()
        {
            commands.AddRange( new List<Keys>() { Keys.Buy, Keys.Sell, Keys.Cancel } ); 
        }
    }
}
