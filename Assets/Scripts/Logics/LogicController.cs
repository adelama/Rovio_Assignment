using System.Collections;
using System.Collections.Generic;
using Rovio.Common;

namespace Rovio.TapMatch.Logic
{
    public class LogicController
    {
        private LevelGrid level;
        internal RandomGenerator Random { get; private set; }

        public LogicController(int widthOfLevel, int heightOfLevel, int numberOfColors, uint randomSeed)
        {
            Random = new RandomGenerator(randomSeed);
            level = new LevelGrid(widthOfLevel, heightOfLevel, numberOfColors, Random);
        }

    }
}
