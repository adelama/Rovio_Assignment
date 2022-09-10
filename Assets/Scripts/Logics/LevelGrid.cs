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
            int topTileIndex;
            int bottomTileIndex;
            int leftTileIndex;
            int rightTileIndex;
            for (int i = 0; i < Tiles.Length; i++)
            {
                topTileIndex = IndexOfNeighborTile(i, LogicConstants.TileNeighbor.Top);
                bottomTileIndex = IndexOfNeighborTile(i, LogicConstants.TileNeighbor.Bottom);
                leftTileIndex = IndexOfNeighborTile(i, LogicConstants.TileNeighbor.Left);
                rightTileIndex = IndexOfNeighborTile(i, LogicConstants.TileNeighbor.Right);
                Tiles[i].SetNeighbors(
                    topTileIndex < 0 ? null : Tiles[topTileIndex],
                    bottomTileIndex < 0 ? null : Tiles[bottomTileIndex],
                    leftTileIndex < 0 ? null : Tiles[leftTileIndex],
                    rightTileIndex < 0 ? null : Tiles[rightTileIndex]
                    );
            }
        }

        /// <summary>
        /// find all color matches of given tile index
        /// </summary>
        /// <param name="tileIndex"></param>
        /// <returns></returns>
        public ColorMatchTiles FindMatchTiles(int tileIndex)
        {
            return FindMatchTilesRecursion(Tiles[tileIndex], new ColorMatchTiles());
        }

        private ColorMatchTiles FindMatchTilesRecursion(Tile tile, ColorMatchTiles matchTiles)
        {
            if (tile == null || matchTiles.Contains(tile))
            {
                return matchTiles;
            }
            matchTiles.AddTile(tile);
            if (tile.IsTopMatch)
            {
                FindMatchTilesRecursion(tile.Top, matchTiles);
            }
            if (tile.IsBottomMatch)
            {
                FindMatchTilesRecursion(tile.Bottom, matchTiles);
            }
            if (tile.IsLeftMatch)
            {
                FindMatchTilesRecursion(tile.Left, matchTiles);
            }
            if (tile.IsRightMatch)
            {
                FindMatchTilesRecursion(tile.Right, matchTiles);
            }
            return matchTiles;
        }

        /// <summary>
        /// returns true if there is any match in neighbors of given tile index
        /// </summary>
        /// <param name="tileIndex"></param>
        /// <returns></returns>
        public bool IsAnyMatch(int tileIndex)
        {
            return Tiles[tileIndex].IsAnyMatch;
        }

        /// <summary>
        /// return index of neighbor tile of target tile if exist,
        /// if index is negative it means there is no tile 
        /// </summary>
        /// <param name="targetTileIndex"></param>
        /// <returns></returns>
        private int IndexOfNeighborTile(int targetTileIndex, LogicConstants.TileNeighbor neighbor)
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
                case LogicConstants.TileNeighbor.Any:
                    throw new System.Exception("Neighbor Any doesn't meant to has an index.");
            }

            return index;
        }

    }
}