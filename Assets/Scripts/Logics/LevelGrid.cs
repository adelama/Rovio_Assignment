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
                Tiles[i] = new Tile(i, (LogicConstants.TileColor)random.Next(numberOfColors));
            }
        }

        public bool HasMatch(Tile target, LogicConstants.TileNeighbor neighbor)
        {
            if (neighbor == LogicConstants.TileNeighbor.All)
            {
                return 
                    HasMatch(target, LogicConstants.TileNeighbor.Top) &&
                    HasMatch(target, LogicConstants.TileNeighbor.Bottom) &&
                    HasMatch(target, LogicConstants.TileNeighbor.Left) &&
                    HasMatch(target, LogicConstants.TileNeighbor.Right);
            }
            int neighborIndex = IndexOfNeighborTile(target.Index, neighbor);
            if (neighborIndex < 0)
            {
                return false;
            }
            return target.Color == Tiles[neighborIndex].Color;
        }

        /// <summary>
        /// return index of neighbor tile of target tile if exist,
        /// if index is negative it means there is no tile 
        /// </summary>
        /// <param name="targetTileIndex"></param>
        /// <returns></returns>
        public int IndexOfNeighborTile(int targetTileIndex, LogicConstants.TileNeighbor neighbor)
        {
            int index = -1;
            switch (neighbor)
            {
                case LogicConstants.TileNeighbor.Top:
                    index = targetTileIndex - width;
                    if (index < 0)
                    {
                        index = -1;
                    }
                    break;
                case LogicConstants.TileNeighbor.Bottom:
                    index = targetTileIndex + width;
                    if (index > Tiles.Length)
                    {
                        index = -1;
                    }
                    break;
                case LogicConstants.TileNeighbor.Left:
                    if (targetTileIndex % width == 0)
                    {
                        index = -1;
                    }
                    else
                    {
                        index = targetTileIndex - 1;
                    }
                    break;
                case LogicConstants.TileNeighbor.Right:
                    index = targetTileIndex + 1;
                    if (index % width == 0)
                    {
                        index = -1;
                    }
                    break;
                case LogicConstants.TileNeighbor.All:
                    throw new System.Exception("Neighbor All doesn't meant to has an index.");
            }

            return index;
        }

    }
}