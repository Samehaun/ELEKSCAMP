using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ELEKSUNI
{
    class InputHandler
    {
        List<ICommand> commands;

        public InputHandler() { }
        public void SetCommand(List<ICommand> commands)
        {
            this.commands = commands;
        }

        public void PoceedInput(int i)
        {
            commands[i].Execute();
        }
    }
}
