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


        public LogicTest()
        {
            logicController = new LogicController(simpleLevelWidth,simpleLevelHeight,simpleLevelNumberOfColors,randomSeed);
            for (int i = 0; i < logicController.Level.Tiles.Length; i++)
            {
                logicController.Level.Tiles[i].SetColor((LogicConstants.TileColor)(i % 3));
            }
        }

        [TestMethod]
        public void InitializeLogic()
        {
            Assert.IsNotNull(logicController);
            Assert.IsNotNull(logicController.Level);
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