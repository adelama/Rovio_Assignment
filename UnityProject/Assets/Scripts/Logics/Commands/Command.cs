using System;

namespace Rovio.TapMatch.Logic
{
    public abstract class Command
    {
        protected LogicController logicController { get; private set; }

        protected Command(LogicController controller)
        {
            this.logicController = controller;
        }

        public abstract bool CanExecute();

        public abstract void Execute();

        public abstract string Serialize();
    }
}
