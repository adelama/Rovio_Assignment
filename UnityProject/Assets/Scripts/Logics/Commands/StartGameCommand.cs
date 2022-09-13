using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rovio.TapMatch.Logic
{
    public class StartGameCommand : Command
    {
        [JsonProperty]
        private int widthOfLevel;
        [JsonProperty]
        private int heightOfLevel;
        [JsonProperty]
        private int numberOfColors;
        [JsonProperty]
        private int randomSeed;
        public StartGameCommand(int widthOfLevel, int heightOfLevel, int numberOfColors, int randomSeed, LogicController logicController)
            : base(logicController)

        {
            this.widthOfLevel = widthOfLevel;
            this.heightOfLevel = heightOfLevel;
            this.numberOfColors = numberOfColors;
            this.randomSeed = randomSeed;
        }

        public override bool CanExecute()
        {
            return !logicController.IsGameStarted;
        }

        public override void Execute()
        {
            logicController.StartGame(widthOfLevel,heightOfLevel,numberOfColors,randomSeed);
        }

        public override string Serialize()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
