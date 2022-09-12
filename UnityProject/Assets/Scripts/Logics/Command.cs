using System;
using System.Diagnostics;
using Rovio.TapMatch.Logic;

namespace Rovio.Common
{
    public abstract class Command
    {
        protected LogicController controller { get; private set; }

        public Command(LogicController controller)
        {
            this.controller = controller;
        }

        public abstract bool CanExecute();

        public abstract void Execute();

    }
}
