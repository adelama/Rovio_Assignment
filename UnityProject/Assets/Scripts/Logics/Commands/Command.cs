using System;
using Newtonsoft.Json;

namespace Rovio.TapMatch.Logic
{
    public abstract class Command
    {
        [JsonIgnore]
        protected LogicController logicController { get; private set; }

        protected Command(LogicController controller)
        {
            this.logicController = controller;
        }

        public abstract bool CanExecute();

        public abstract void Execute();

        public abstract string Serialize();

        public static Command Deserialize(string objJson,LogicController logicController)
        {
            Command cmd = JsonConvert.DeserializeObject<Command>(objJson);
            cmd.logicController = logicController;
            return cmd;
        }
    }
}
