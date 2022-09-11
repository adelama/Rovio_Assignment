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
        }

        [TestMethod]
        public void Initialize()
        {
            Assert.IsNotNull(logicController);
        }
    }
}