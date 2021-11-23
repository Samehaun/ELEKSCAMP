using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ELEKSUNI
{
    public class Report
    {
        private string language;
        public string Message { get; private set; }
        public string PlayerState { get; private set; }
        public List<string> Options { get; private set; }
        public Report()
        {
            Options = new List<string>();
        }
        internal void SetReportMessage(Keys key)
        {
            Message = Data.Localize(key, language);
        }
        internal void SetReportMessage(string message)
        {
            Message = message;
        }
        internal void ClearReportMessage()
        {
            Message = null;
        }
        internal void AddNewLineMessage(Keys key)
        {
            Message = $"{ Message }{ Environment.NewLine }{ Data.Localize(key, language) }";
        }
        internal void AppendRepportMessage(Keys key)
        {
            Message = $"{ Message }{ Data.Localize(key, language) }";
        }
        internal void RefreshPlayerState(Player player)
        {
            PlayerState = Data.PlayerStateBuilder(player, language);
        }
        internal void ResetOptions(List<Keys> options)
        {
            Options.Clear();
            Options.AddRange(Data.Localize(options, language));
        }
        internal void ResetOptions(List<string> options)
        {
            Options.Clear();
            Options.AddRange(options);
        }
        internal void SetLanguage(string language)
        {
            this.language = language;
        }
        internal void EndingReport(Keys result)
        {
            AppendRepportMessage(result);
            PlayerState = null;
            Options = null;
        }
        internal void ListInventoryForUseAndLoot(Inventory inventory, Player player)
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
            ResetOptions(itemsDescriptions);
        }
        internal void ListInventoryForTrading(Inventory inventory, Player player)
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
            ResetOptions(itemsDescriptions);
        }
    }
}