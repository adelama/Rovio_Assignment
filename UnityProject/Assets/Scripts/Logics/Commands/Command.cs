using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Rovio.TapMatch.Logic
{
    public abstract class Command
    {
        protected enum CommandType { StartGame, PopTile }

        [JsonProperty]
        private CommandType commandType;

        [JsonIgnore]
        protected LogicController logicController { get; private set; }

        protected Command(LogicController controller, CommandType type)
        {
            this.logicController = controller;
            commandType = type;
        }

        public abstract bool CanExecute();

        public abstract void Execute();

        public abstract string Serialize();

        public static Command Deserialize(string strJson, LogicController logicController)
        {
            Command cmd = null;
            CommandType commandType = (CommandType)JObject.Parse(strJson).GetValue("commandType").Value<int>();
            switch (commandType)
            {
                case CommandType.StartGame:
                    cmd = JsonConvert.DeserializeObject<StartGameCommand>(strJson);
                    break;
                case CommandType.PopTile:
                    cmd = JsonConvert.DeserializeObject<PopTileCommand>(strJson);
                    break;
            }
            cmd.logicController = logicController;
            return cmd;
        }
    }
}
