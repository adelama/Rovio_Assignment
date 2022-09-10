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
        private RandomGenerator random;
        public Tile[] Tiles { get; private set; }

        public LevelGrid(int width, int height, int numberOfColors, RandomGenerator random)
        {
            this.width = width;
            this.height = height;
            this.numberOfColors = numberOfColors;
            this.random = random;
            Tiles = new Tile[width * height];
            for (int i = 0; i < Tiles.Length; i++)
            {
                Tiles[i] = new Tile(i,(LogicConstants.TileColor)random.Next(numberOfColors));
            }
        }



    }
}