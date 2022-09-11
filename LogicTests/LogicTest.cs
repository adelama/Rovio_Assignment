using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rovio.TapMatch.Logic;

namespace LogicTests
{
    [TestClass]
    public class LogicTest
    {
        private const int randomSeed = 123456;
        private const int simpleLevelWidth = 4;
        private const int simpleLevelHeight = 3;
        private const int simpleLevelNumberOfColors = 3;

        private LogicController logicController;

        //The RandomGenerator is Deterministic so
        //for randomSeed = 123456 the Initial Level State Should be like this
        /*
              ---------------------------------------------
             | 0: Green |  1: Red  |  2: Red  |  3: Red    |
              ---------------------------------------------
             |  4: Red  |  5: Red  |  6: Blue |  7: Red    |
              ---------------------------------------------
             |  8: Blue | 9: Green |  10: Red  |  11: Blue |
              ---------------------------------------------
         */

        public LogicTest()
        {
            logicController = new LogicController(simpleLevelWidth, simpleLevelHeight, simpleLevelNumberOfColors, randomSeed);
            Console.WriteLine("Initial Level State:");
            PrintCurrentLevelTilesColor();
        }

        private void PrintCurrentLevelTilesColor()
        {
            Console.WriteLine("_______________________________________________________");
            for (int i = 0; i < logicController.Level.Tiles.Length; i++)
            {
                if (i % simpleLevelWidth == 0)
                {
                    Console.WriteLine();
                }
                Console.Write($"{i}: {logicController.Level.Tiles[i].Color} | ");
            }
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
            Tile tile = logicController.Level.Tiles[5];
            Assert.AreEqual(logicController.Level.Tiles[1], tile.Top);
            Assert.AreEqual(logicController.Level.Tiles[9], tile.Bottom);
            Assert.AreEqual(logicController.Level.Tiles[4], tile.Left);
            Assert.AreEqual(logicController.Level.Tiles[6], tile.Right);
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
            int tileIndex = 5;
            Assert.IsTrue(logicController.Level.Tiles[tileIndex].IsTopMatch);
            Assert.IsTrue(logicController.Level.Tiles[tileIndex].IsLeftMatch);
            Assert.IsFalse(logicController.Level.Tiles[tileIndex].IsBottomMatch);
            Assert.IsFalse(logicController.Level.Tiles[tileIndex].IsRightMatch);

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

        private LogicController LogicWithDeadLockStateLevel()
        {
            LogicController controller = new LogicController(3, 3, 2, randomSeed);
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

    }
}