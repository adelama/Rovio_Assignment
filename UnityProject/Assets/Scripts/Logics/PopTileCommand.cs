using System;

namespace Rovio.TapMatch.Logic
{
    public class PopTileCommand : Command
    {
        private int tileIndex;
        private ColorMatchTiles colorMatchTiles;
        public PopTileCommand(int tileIndex, LogicController controller) : base(controller)
        {
            this.tileIndex = tileIndex;
            colorMatchTiles = controller.Level.FindMatchTiles(tileIndex);
        }
        public override bool CanExecute()
        {
            return colorMatchTiles.TilesCount > 1;
        }

        public override void Execute()
        {
            controller.PopMatchTiles(colorMatchTiles);
        }
    }
}
