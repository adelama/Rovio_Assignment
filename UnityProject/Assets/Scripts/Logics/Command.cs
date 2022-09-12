using System;
using System.Diagnostics;

namespace Rovio.TapMatch.Logic
{
    public abstract class Command
    {
        protected LogicController controller { get; private set; }

        protected Command(LogicController controller)
        {
            this.controller = controller;
        }

        public abstract bool CanExecute();

        public abstract void Execute();

    }
}
