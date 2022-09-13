using System;

namespace Rovio.TapMatch.Logic
{
    public class PopTileCommand : Command
    {
        private int tileIndex;
        public ColorMatchTiles ColorMatchTiles { get; private set; }
        public PopTileCommand(int tileIndex, LogicController controller) : base(controller)
        {
            this.tileIndex = tileIndex;
            ColorMatchTiles = controller.Level.FindMatchTiles(tileIndex);
        }
        public override bool CanExecute()
        {
            return ColorMatchTiles.TilesCount > 1;
        }

        public override void Execute()
        {
            logicController.PopMatchTiles(ColorMatchTiles);
        }

        public override string Serialize()
        {
            throw new NotImplementedException();
        }
    }
}
