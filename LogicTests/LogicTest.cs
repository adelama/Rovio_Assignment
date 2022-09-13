using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rovio.TapMatch.Logic;

namespace LogicTests
{
    [TestClass]
    public class LogicTest
    {
        private const int randomSeed = 123456;
        private const int simpleLevelWidth = 5;
        private const int simpleLevelHeight = 5;
        private const int simpleLevelNumberOfColors = 3;

        private LogicController logicController;

        //The RandomGenerator is Deterministic so
        //for randomSeed = 123456 the Initial Level State Should be like this
        /*
              --------------------------------------------------------
             |  0: Green | 1: Red   | 2: Red    | 3: Red   | 4: Red   |
              --------------------------------------------------------
             |  5: Red   | 6: Blue  | 7: Red    | 8: Blue  | 9: Green |
              --------------------------------------------------------
             |  10: Red  | 11: Blue | 12: Red   | 13: Red  | 14: Red  |
              --------------------------------------------------------
             |  15: Red  | 16: Blue | 17: Green | 18: Red  | 19: Blue |
              --------------------------------------------------------
             |  20: Red  | 21: Red  | 22: Green | 23: Blue | 24: Blue |
              --------------------------------------------------------
         */

        public LogicTest()
        {
            logicController = new LogicController();
            var startCmd = new StartGameCommand(simpleLevelWidth, simpleLevelHeight, simpleLevelNumberOfColors, randomSeed, logicController);
            logicController.ExecuteCommand(startCmd);
            Console.WriteLine("Initial Level State:");
            PrintCurrentLevelTilesColor();
        }

        private void PrintCurrentLevelTilesColor()
        {
            Console.WriteLine("__________________________________________________________");
            for (int i = 0; i < logicController.Level.Tiles.Length; i++)
            {
                if (i % simpleLevelWidth == 0 && i != 0)
                {
                    Console.WriteLine();
                }
                Console.Write($"{i}: {logicController.Level.Tiles[i].Color}".PadRight(10)+"| ");
            }
            Console.WriteLine();
            Console.WriteLine("----------------------------------------------------------");
        }

        [TestMethod]
        public void InitializeLogic()
        {
            Assert.IsNotNull(logicController);
            Assert.IsNotNull(logicController.Level);
        }

        [TestMethod]
        public void CheckingTileNeighbors()
        {
            Tile tile = logicController.Level.Tiles[7];
            Assert.AreEqual(logicController.Level.Tiles[2], tile.Top);
            Assert.AreEqual(logicController.Level.Tiles[12], tile.Bottom);
            Assert.AreEqual(logicController.Level.Tiles[6], tile.Left);
            Assert.AreEqual(logicController.Level.Tiles[8], tile.Right);
        }

        [TestMethod]
        public void CheckingCornerTilesNeighbors()
        {
            //Top Left Tile
            int tileIndex = 0;
            Tile tile = logicController.Level.Tiles[tileIndex];
            Assert.AreEqual(null, tile.Top);
            Assert.AreEqual(logicController.Level.Tiles[tileIndex + simpleLevelWidth], tile.Bottom);
            Assert.AreEqual(null, tile.Left);
            Assert.AreEqual(logicController.Level.Tiles[tileIndex + 1], tile.Right);

            //Top Right Tile
            tileIndex = simpleLevelWidth - 1;
            tile = logicController.Level.Tiles[tileIndex];
            Assert.AreEqual(null, tile.Top);
            Assert.AreEqual(logicController.Level.Tiles[tileIndex + simpleLevelWidth], tile.Bottom);
            Assert.AreEqual(logicController.Level.Tiles[tileIndex - 1], tile.Left);
            Assert.AreEqual(null, tile.Right);

            //Down Left Tile
            tileIndex = (simpleLevelHeight - 1) * simpleLevelWidth;
            tile = logicController.Level.Tiles[tileIndex];
            Assert.AreEqual(logicController.Level.Tiles[tileIndex - simpleLevelWidth], tile.Top);
            Assert.AreEqual(null, tile.Bottom);
            Assert.AreEqual(null, tile.Left);
            Assert.AreEqual(logicController.Level.Tiles[tileIndex + 1], tile.Right);

            //Down Right Tile
            tileIndex = logicController.Level.Tiles.Length - 1;
            tile = logicController.Level.Tiles[tileIndex];
            Assert.AreEqual(logicController.Level.Tiles[tileIndex - simpleLevelWidth], tile.Top);
            Assert.AreEqual(null, tile.Bottom);
            Assert.AreEqual(logicController.Level.Tiles[tileIndex - 1], tile.Left);
            Assert.AreEqual(null, tile.Right);
        }

        [TestMethod]
        public void IsNeighborsMatchForTile()
        {
            int tileIndex = 13;
            Assert.IsFalse(logicController.Level.Tiles[tileIndex].IsTopMatch);
            Assert.IsTrue(logicController.Level.Tiles[tileIndex].IsLeftMatch);
            Assert.IsTrue(logicController.Level.Tiles[tileIndex].IsBottomMatch);
            Assert.IsTrue(logicController.Level.Tiles[tileIndex].IsRightMatch);

            tileIndex = 0;
            Assert.IsFalse(logicController.Level.Tiles[tileIndex].IsTopMatch);
            Assert.IsFalse(logicController.Level.Tiles[tileIndex].IsLeftMatch);
            Assert.IsFalse(logicController.Level.Tiles[tileIndex].IsBottomMatch);
            Assert.IsFalse(logicController.Level.Tiles[tileIndex].IsRightMatch);

        }

        [TestMethod]
        public void IsAnyMatchForTile()
        {
            int tileIndex = 0;
            Assert.IsFalse(logicController.Level.IsAnyMatch(tileIndex));
            tileIndex = 2;
            Assert.IsTrue(logicController.Level.IsAnyMatch(tileIndex));
        }

        [TestMethod]
        public void FindingTilesMatchColorOfGivenTile()
        {
            //expected tiles index: 1, 2, 3, 4, 7, 12, 13, 14, 18
            ColorMatchTiles expectedMatches = new ColorMatchTiles();
            expectedMatches.AddTile(logicController.Level.Tiles[1]);
            expectedMatches.AddTile(logicController.Level.Tiles[2]);
            expectedMatches.AddTile(logicController.Level.Tiles[3]);
            expectedMatches.AddTile(logicController.Level.Tiles[4]);
            expectedMatches.AddTile(logicController.Level.Tiles[7]);
            expectedMatches.AddTile(logicController.Level.Tiles[12]);
            expectedMatches.AddTile(logicController.Level.Tiles[13]);
            expectedMatches.AddTile(logicController.Level.Tiles[14]);
            expectedMatches.AddTile(logicController.Level.Tiles[18]);
            for (int tileIndex = 1; tileIndex < 7; tileIndex++)
            {
                int tileIndexToStartFinding = tileIndex;
                if (tileIndex == 5)
                    tileIndexToStartFinding = 7;
                else if (tileIndex == 6)
                    tileIndexToStartFinding = 18;
                ColorMatchTiles foundMatches = logicController.Level.FindMatchTiles(tileIndexToStartFinding);
                var foundedTilesArray = foundMatches.TilesSortedArray;
                var expectedTilesArray = expectedMatches.TilesSortedArray;
                Console.WriteLine("Founded Tiles Match Color of Tile: " + tileIndexToStartFinding);
                for (int i = 0; i < foundedTilesArray.Length; i++)
                {
                    Console.Write($"{foundedTilesArray[i].Index}, ");
                    Assert.AreEqual(expectedTilesArray[i], foundedTilesArray[i]);
                }
                Console.WriteLine();
            }
        }

        private LogicController LogicWithDeadLockStateLevel()
        {
            LogicController controller = new LogicController();
            var startCmd = new StartGameCommand(5, 5, 3, randomSeed, controller);
            controller.ExecuteCommand(startCmd);
            for (int i = 0; i < controller.Level.Tiles.Length; i++)
            {
                controller.Level.Tiles[i].SetColor((LogicConstants.TileColor)(i % 2));
            }
            return controller;
        }

        [TestMethod]
        public void IsLevelAtDeadLockTest()
        {
            Assert.IsTrue(LogicWithDeadLockStateLevel().Level.IsAtDeadLock);
            Assert.IsFalse(logicController.Level.IsAtDeadLock);
        }

        [TestMethod]
        public void SolveDeadLockByShuffling()
        {
            LogicController controllerWithDeadLevel = LogicWithDeadLockStateLevel();
            controllerWithDeadLevel.CheckAndSolveDeadLock();
            Assert.IsFalse(controllerWithDeadLevel.Level.IsAtDeadLock);
        }

        [TestMethod]
        public void CanExecutePopTileCommand()
        {
            int tileIndex = 8;
            Assert.IsFalse(logicController.ExecuteCommand(new PopTileCommand(tileIndex, logicController)));
            tileIndex = 7;
            Assert.IsTrue(logicController.ExecuteCommand(new PopTileCommand(tileIndex, logicController)));
        }

        [TestMethod]
        public void CheckingPopTileCommand()
        {
            int tileIndex = 13;
            string[] expectedTilesState = new string[]
                {
                    "Green", "Green", "Red"   , "Red"  , "Red"   ,
                    "Red"  , "Blue" , "Blue"  , "Red"  , "Green" ,
                    "Red"  , "Blue" , "Green" , "Red"  , "Green" ,
                    "Red"  , "Blue" , "Green" , "Blue" , "Blue"  ,
                    "Red"  , "Red"  , "Green" , "Blue" , "Blue"  ,
                };
            logicController.ExecuteCommand(new PopTileCommand(tileIndex, logicController));
            for (int i = 0; i < expectedTilesState.Length; i++)
            {
                Assert.IsTrue(expectedTilesState[i] == logicController.Level.Tiles[i].Color.ToString());
            }
            Console.WriteLine();
            Console.WriteLine("Level State after Popping Tile: " + tileIndex);
            PrintCurrentLevelTilesColor();
        }

    }
}