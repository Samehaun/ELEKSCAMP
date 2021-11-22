using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ELEKSUNI
{
    class Report
    {
        private string language;
        public string Message { get; private set; }
        public string PlayerState { get; private set; }
        public List<string> Options { get; set; }
        public Report()
        {
            Options = new List<string>();
        }
        public void SetReportMessage(Keys key)
        {
            Message = Data.Localize(key, language);
        }
        public void AddNewLineMessage(Keys key)
        {
            Message = $"{ Message }{ Environment.NewLine }{ Data.Localize(key, language) }";
        }
        public void AppendRepportMessage(Keys key)
        {
            Message = $"{ Message }{ Data.Localize(key, language) }";
        }
        public void RefreshPlayerState(Player player)
        {
            PlayerState = Data.PlayerStateBuilder(player, language);
        }
        public void ResetOptions(List<Keys> options)
        {
            Options.Clear();
            Options.AddRange(Data.Localize(options, language));
        }
        public void ResetOptions(List<string> options)
        {
            Options.Clear();
            Options.AddRange(options);
        }
    }
}