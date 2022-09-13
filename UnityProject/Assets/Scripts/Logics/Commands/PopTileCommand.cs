using System;
using Newtonsoft.Json;

namespace Rovio.TapMatch.Logic
{
    public class PopTileCommand : Command
    {
        [JsonProperty]
        private int tileIndex;
        [JsonIgnore]
        public ColorMatchTiles ColorMatchTiles { get; private set; }

        public PopTileCommand(int tileIndex, LogicController controller) : base(controller)
        {
            this.tileIndex = tileIndex;
        }

        private ColorMatchTiles GetColorMatchTiles()
        {
            if (ColorMatchTiles == null)
            {
                ColorMatchTiles = logicController.Level.FindMatchTiles(tileIndex);
            }
            return ColorMatchTiles;
        }

        public override bool CanExecute()
        {
            return GetColorMatchTiles().TilesCount > 1;
        }

        public override void Execute()
        {
            logicController.PopMatchTiles(GetColorMatchTiles());
        }

        public override string Serialize()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
