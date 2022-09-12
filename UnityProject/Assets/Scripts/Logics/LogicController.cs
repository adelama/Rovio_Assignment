using System.Collections;
using System.Collections.Generic;
using Rovio.Common;

namespace Rovio.TapMatch.Logic
{
    public class LogicController
    {
        public LevelGrid Level { get; private set; }
        public RandomGenerator Random { get; private set; }

        public LogicController(int widthOfLevel, int heightOfLevel, int numberOfColors, int randomSeed)
        {
            Random = new RandomGenerator(randomSeed);
            Level = new LevelGrid(widthOfLevel, heightOfLevel, numberOfColors, Random);
            CheckAndSolveDeadLock();
        }

        public void CheckAndSolveDeadLock()
        {
            while (Level.IsAtDeadLock)
            {
                Level.Shuffle();        
            } 
        }


        //this is require to run tests
        private static void Main()
        {
            
        }
    }
}
