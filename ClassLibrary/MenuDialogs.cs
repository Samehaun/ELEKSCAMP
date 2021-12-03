using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ELEKSUNI
{
    abstract class MenuDialogs
    {
        protected Report report;
        protected List<Keys> commands;
        protected Player player;
        protected Spot currentSpot;

        public MenuDialogs(Quest quest)
        {
            report = quest.report;
            commands = quest.availableCommands;
            player = quest.player;
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
        abstract protected void SetAvailableOptions();
        virtual protected void SetOptionsView()
        {
            report.ResetOptions(commands);
        }
    }
    class ItemDialog : MenuDialogs
    {
        public ItemDialog(Quest quest) : base(quest) { }
        protected override void SetMessage()
        {
            report.SetReportMessage(player.GetActiveItem().Name);
        }
        protected override void SetAvailableOptions()
        {
            commands.AddRange(player.GetActiveItem().Options);
        }
    }
    class TravelDialog : MenuDialogs
    {       
        public TravelDialog(Quest quest) : base(quest) { }
        protected override void SetMessage()
        {
            report.SetReportMessage(Keys.DirectionDialogMessage);
        }
        protected override void SetAvailableOptions()
        {
            commands.AddRange(currentSpot.GetAvailableDirections());
        }
    }
    class MainDialog : MenuDialogs
    {
        public MainDialog(Quest quest) : base(quest) { }
        protected override void SetMessage()
        {
            report.SetReportMessage(Keys.DirectionDialogMessage);
        }
        protected override void SetAvailableOptions()
        {
            commands.AddRange(currentSpot.GetAvailableDirections());
        }

    }
}
