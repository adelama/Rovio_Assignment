using System;
using System.Collections;
using System.Collections.Generic;
using Rovio.Common;

namespace Rovio.TapMatch.Logic
{
    public class LevelGrid
    {
        public int Width { get; private set; }
        public int Height { get; private set; }
        public int NumberOfColors { get; private set; }
        public Tile[] Tiles { get; private set; }

        private RandomGenerator random;

        public bool IsAtDeadLock
        {
            get
            {
                for (int i = 0; i < Tiles.Length; i++)
                {
                    if (Tiles[i].IsAnyMatch)
                    {
                        return false;
                    }
                }
                return true;
            }
        }


        internal LevelGrid(int width, int height, int numberOfColors, RandomGenerator random)
        {
            this.Width = width;
            this.Height = height;
            this.NumberOfColors = numberOfColors;
            this.random = random;
            Tiles = new Tile[width * height];
            for (int i = 0; i < Tiles.Length; i++)
            {
                Tiles[i] = new Tile(i, GetRandomTileColor());
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

        private LogicConstants.TileColor GetRandomTileColor()
        {
            return (LogicConstants.TileColor)random.Next(NumberOfColors);
        }


        internal void Shuffle()
        {
            for (int i = 0; i < Tiles.Length; i++)
            {
                Tile.SwapColor(Tiles[i], Tiles[random.Next(Tiles.Length)]);
            }
        }

        internal void DropDownTiles()
        {
            for (int i = Tiles.Length - 1; i >= Tiles.Length - Width; i--)
            {
                DropDownTilesInColumnRecursion(Tiles[i]);
            }
        }

        private void DropDownTilesInColumnRecursion(Tile tile)
        {
            if (tile == null)
            {
                return;
            }
            DropDownSingleTile(tile);
            DropDownTilesInColumnRecursion(tile.Top);
            if (tile.Color == LogicConstants.TileColor.None)
            {
                tile.SetColor(GetRandomTileColor());
            }
        }

        private void DropDownSingleTile(Tile tile)
        {
            if (tile.Color != LogicConstants.TileColor.None)
            {
                while (tile.Bottom != null &&
                        tile.Bottom.Color == LogicConstants.TileColor.None)
                {
                    Tile.SwapColor(tile, tile.Bottom);
                    tile = tile.Bottom;
                }
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
                    index = targetTileIndex - Width;
                    if (index < 0)
                    {
                        index = -1;
                    }
                    break;
                case LogicConstants.TileNeighbor.Bottom:
                    index = targetTileIndex + Width;
                    if (index >= Tiles.Length)
                    {
                        index = -1;
                    }
                    break;
                case LogicConstants.TileNeighbor.Left:
                    if (targetTileIndex % Width == 0)
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
                    if (index % Width == 0)
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