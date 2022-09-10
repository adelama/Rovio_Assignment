using System.Collections;
using System.Collections.Generic;

namespace Rovio.TapMatch.Logic
{
    public class Tile
    {
        internal int Id { get; private set; }

        //the index may change in gameplay
        internal int Index { get; set; }
        internal LogicConstants.TileColor Color { get; private set; }

        public Tile(int index, LogicConstants.TileColor color)
        {
            this.Id = index;
            this.Index = index;
            this.Color = color;
        }

    }
}