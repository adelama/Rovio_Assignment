using System.Collections;
using System.Collections.Generic;
using Rovio.Common;

namespace Rovio.TapMatch.Logic
{
    public class LevelGrid
    {
        private int width;
        private int height;
        private int numberOfColors;
        public Tile[] Tiles { get; private set; }

        public LevelGrid(int width, int height, int numberOfColors)
        {
            this.width = width;
            this.height = height;
            this.numberOfColors = numberOfColors;
        }

    }
}