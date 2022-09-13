using System;
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
            widthOfLevel = Utils.Clamp(widthOfLevel,LogicConstants.MinLevelWidth,LogicConstants.MaxLevelWidth);
            heightOfLevel = Utils.Clamp(heightOfLevel, LogicConstants.MinLevelHeight,LogicConstants.MaxLevelHeight);
            numberOfColors = Utils.Clamp(numberOfColors, LogicConstants.MinColorsType,LogicConstants.MaxColorsType);
            Level = new LevelGrid(widthOfLevel, heightOfLevel, numberOfColors, Random);
            CheckAndSolveDeadLock();
        }



        /// <summary>
        /// execute a command to proceed in logic
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns>return true if command executed successfully</returns>
        public bool ExecuteCommand(Command cmd)
        {
            if (cmd.CanExecute())
            {
                cmd.Execute();
                return true;
            }
            return false;
        }

        public void CheckAndSolveDeadLock()
        {
            while (Level.IsAtDeadLock)
            {
                Level.Shuffle();        
            } 
        }

        public void PopMatchTiles(ColorMatchTiles matchTiles)
        {
            Tile[] tiles = matchTiles.TilesArray;
            for (int i = 0; i < tiles.Length; i++)
            {
                tiles[i].SetColor(LogicConstants.TileColor.None);
            }
            Level.DropDownTiles();
            CheckAndSolveDeadLock();
        }




        //this is require to run tests
        private static void Main()
        {
            
        }
    }
}
