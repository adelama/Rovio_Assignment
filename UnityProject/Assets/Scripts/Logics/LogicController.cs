using System;
using System.Collections.Generic;
using Rovio.Common;

namespace Rovio.TapMatch.Logic
{
    public class LogicController
    {
        public LevelGrid Level { get; private set; }
        internal RandomGenerator Random { get; private set; }

        public bool IsGameStarted { get; private set; }

        public List<Command> ExecutedCommands { get; private set; }

        private Action<ColorMatchTiles> onLevelUpdate;
        private Action onGameStart;

        public LogicController(Action onGameStart=null, Action<ColorMatchTiles> onLevelUpdate = null)
        {
            ExecutedCommands = new List<Command>();
            this.onLevelUpdate = onLevelUpdate;
            this.onGameStart = onGameStart;
        }

        internal void StartGame(int widthOfLevel, int heightOfLevel, int numberOfColors, int randomSeed)
        {
            Random = new RandomGenerator(randomSeed);
            widthOfLevel = Utils.Clamp(widthOfLevel, LogicConstants.MinLevelWidth, LogicConstants.MaxLevelWidth);
            heightOfLevel = Utils.Clamp(heightOfLevel, LogicConstants.MinLevelHeight, LogicConstants.MaxLevelHeight);
            numberOfColors = Utils.Clamp(numberOfColors, LogicConstants.MinColorsType, LogicConstants.MaxColorsType);
            Level = new LevelGrid(widthOfLevel, heightOfLevel, numberOfColors, Random);
            CheckAndSolveDeadLock();
            IsGameStarted = true;
            onGameStart();
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
                ExecutedCommands.Add(cmd);
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

        internal void PopMatchTiles(ColorMatchTiles matchTiles)
        {
            Tile[] tiles = matchTiles.TilesArray;
            for (int i = 0; i < tiles.Length; i++)
            {
                tiles[i].SetColor(LogicConstants.TileColor.None);
            }
            Level.DropDownTiles();
            CheckAndSolveDeadLock();
            onLevelUpdate?.Invoke(matchTiles);
        }




        //this is require to run tests
        private static void Main()
        {

        }
    }
}
