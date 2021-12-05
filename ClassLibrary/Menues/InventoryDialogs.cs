using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ELEKSUNI
{
    abstract class InventoryDialogs
    {
        protected Report report;
        protected List<Keys> commands;
        protected List<Item> itemsToShow;
        protected Player player;
        protected Keys header;
        protected Keys option;
        protected Func<Item, string> mode;
        public InventoryDialogs(Quest quest)
        {
            report = quest.report;
            player = quest.player;
            commands = quest.availableCommands;
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
            commands = GetListOfSameOptions();
            commands.Add(Keys.Cancel);
        }
        virtual protected void SetOptionsView()
        {
            report.ShowInventory(itemsToShow, player, mode);
        }
        protected List<Keys> GetListOfSameOptions()
        {
            List<Keys> repeatingList = Enumerable.Repeat(option, itemsToShow.Count).ToList();
            return repeatingList;
        }
    }
    class OpenInventoryDialog : InventoryDialogs
    {
        public OpenInventoryDialog(Quest quest) : base(quest)
        {
            header = Keys.Inventory;
            itemsToShow = player.GetListOfItemsInInventory();
            option = Keys.Select;
            mode = report.ItemSpecs;
        }
    }
    class BuyDialog : InventoryDialogs
    {
        public BuyDialog(Quest quest) : base(quest)
        {
            header = Keys.Buy;
            itemsToShow = quest.questMap.PlayerSpot.npc.GetListOfItemsInInventory();
            option = Keys.Buy;
            mode = report.ItemSpecsForTrading;
        }
    }
    class SellDialog : InventoryDialogs
    {
        public SellDialog(Quest quest) : base(quest)
        {
            header = Keys.Sell;
            itemsToShow = player.GetListOfItemsInInventory();
            option = Keys.Sell;
            mode = report.ItemSpecsForTrading;
        }
    }
    class StealDialog : InventoryDialogs
    {
        public StealDialog(Quest quest) : base(quest)
        {
            header = Keys.Steal;
            itemsToShow = quest.questMap.PlayerSpot.npc.GetListOfItemsInInventory();
            option = Keys.StealItem;
            mode = report.ItemSpecs;
        }
    }
    class LootDialog : InventoryDialogs
    {
        public LootDialog(Quest quest) : base(quest)
        {
            header = Keys.Loot;
            itemsToShow = quest.questMap.PlayerSpot.npc.GetListOfItemsInInventory();
            option = Keys.LootItem;
            mode = report.ItemSpecs;
        }
        override protected void SetAvailableOptions()
        {
            commands = GetListOfSameOptions();
            commands.Add(Keys.Main);
        }
    }
}
