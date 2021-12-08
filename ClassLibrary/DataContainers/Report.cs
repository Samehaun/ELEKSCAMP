using System;
using System.Collections.Generic;

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
        internal ReportSave Save()
        {
            return new ReportSave(Message, language, Options);
        }
        internal void Load(ReportSave save)
        {
            Message = save.Message;
            language = save.Language;
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
            if (Message != null)
            {
                Message = $"{ Message }{ Environment.NewLine }{ Data.Localize(key, language) }";
            }
            else
            {
                Message = $"{ Data.Localize(key, language) }";
            }

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
            SetReportMessage(Keys.InitialMessage);
        }
        internal void EndingReport(Keys result)
        {
            AddNewLineMessage(result);
            PlayerState = null;
            Options = null;
        }
        internal void ShowInventory(List<Item> items, Player player, Func<Item, string> itemSpecShowMode)
        {
            List<string> itemsDescriptions = new List<string>();
            foreach (var item in items)
            {
                if (item == player.CurrentClothes || item == player.CurrentWeapon)
                {
                    itemsDescriptions.Add($"{ itemSpecShowMode(item) } *{Data.Localize(Keys.Equipped, language)}*");
                }
                else
                {
                    itemsDescriptions.Add(itemSpecShowMode(item));
                }
            }
            if (itemSpecShowMode == ItemSpecsForTrading)
            {
                PlayerState = $"{ Data.Localize(Keys.Remains, language) } { player.HowMuchCoins() } { Data.Localize(Keys.Coins, language) }";
            }
            itemsDescriptions.Add(Data.Localize(Keys.Cancel, language));
            ResetOptions(itemsDescriptions);
        }
        internal string ItemSpecs(Item item)
        {
            return item.GetItemSpecs(language);
        }
        internal string ItemSpecsForTrading(Item item)
        {
            return item.GetItemSpecsForTrade(language);
        }
    }
    struct ReportSave
    {
        public string Message { get; set; }
        public string Language { get; set; }
        public List<string> Options { get; set; }
        public ReportSave(string message, string langSettings, List<string> options)
        {
            Options = new List<string>();
            Options.AddRange(options);
            Message = message;
            Language = langSettings;

        }
    }
}