using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ELEKSUNI
{
    class InputHandler
    {
        List<Command> commands;
        Stack<Command> history;
        public InputHandler() 
        {
            history = new Stack<Command>();
        }
        public void SetCommands(List<Command> commands)
        {
            this.commands = commands;
        }
        public void PoceedInput(int i)
        {
            commands[i].Execute();
        }
        public void ResetCommandsHistory(Command command)
        {
            history.Clear();
            history.Push(command);
        }
        public void DiveInNestedMenu(Command command)
        {
            if(history.Peek() != command)
            {
                history.Push(command);
            }
        }
        public void PreviousMenu()
        {
            history.Pop().Execute();
        }
    }
}
