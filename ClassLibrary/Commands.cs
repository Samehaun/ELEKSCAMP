using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ELEKSUNI
{
    interface ICommand
    {
        public void Execute(Player player, Map map, Time time, Report report);
    }
    public class Commands
    {

    }
    class StartHandlingIntInputCommand : ICommand
    {
        public void Execute(Player player, Map map, Time time, Report report)
        {
            report.SetReportMessage(Keys.InitialMessage);
            if (map.PlayerSpot.npc != null && map.PlayerSpot.npc.IsHostile)
            {
                report.AddNewLineMessage(Keys.Enemy);
                report.AppendRepportMessage(map.PlayerSpot.npc.Name);
            }
            LaunchMainDialogCommand.();
        }
    }
    class LaunchMainDialogCommand : ICommand
    {
        public void Execute(Player player, Map map, Time time, Report report)
        {

        }
    }
}
