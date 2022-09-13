using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rovio.TapMatch.Logic
{
    public class StartGameCommand : Command
    {
        private int widthOfLevel;
        private int heightOfLevel;
        private int numberOfColors;
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
    }
}
