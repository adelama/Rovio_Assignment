using System.Collections;
using System.Collections.Generic;

namespace Rovio.TapMatch.Logic
{
    public class Tile
    {
        public int Index { get; private set; }
        public LogicConstants.TileColor Color { get; private set; }

        public Tile Top { get; private set; }
        public Tile Bottom { get; private set; }
        public Tile Left { get; private set; }
        public Tile Right { get; private set; }
        public bool IsAnyMatch => IsMatchNeighbor(LogicConstants.TileNeighbor.Any);
        public bool IsTopMatch => IsMatchNeighbor(LogicConstants.TileNeighbor.Top);
        public bool IsBottomMatch => IsMatchNeighbor(LogicConstants.TileNeighbor.Bottom);
        public bool IsLeftMatch => IsMatchNeighbor(LogicConstants.TileNeighbor.Left);
        public bool IsRightMatch => IsMatchNeighbor(LogicConstants.TileNeighbor.Right);


        public Tile(int index, LogicConstants.TileColor color)
        {
            this.Index = index;
            this.Color = color;
        }

        public void SetColor(LogicConstants.TileColor color)
        {
            this.Color = color;
        }

        public void SetNeighbors(
            Tile top,
            Tile bottom,
            Tile left,
            Tile right
            )
        {
            this.Top = top;
            this.Bottom = bottom;
            this.Left = left;
            this.Right = right;
        }

        private bool IsMatchNeighbor(LogicConstants.TileNeighbor neighbor)
        {
            Tile tile = null;
            switch (neighbor)
            {
                case LogicConstants.TileNeighbor.Top:
                    tile = Top;
                    break;
                case LogicConstants.TileNeighbor.Bottom:
                    tile = Bottom;
                    break;
                case LogicConstants.TileNeighbor.Left:
                    tile = Left;
                    break;
                case LogicConstants.TileNeighbor.Right:
                    tile = Right;
                    break;
                case LogicConstants.TileNeighbor.Any:
                    return
                        IsMatchNeighbor(LogicConstants.TileNeighbor.Top) ||
                        IsMatchNeighbor(LogicConstants.TileNeighbor.Bottom) ||
                        IsMatchNeighbor(LogicConstants.TileNeighbor.Left) ||
                        IsMatchNeighbor(LogicConstants.TileNeighbor.Right);
            }
            if (tile == null)
            {
                return false;
            }
            return tile.Color == Color;
        }

        public static void SwapColor(Tile t1, Tile t2)
        {
            LogicConstants.TileColor color1 = t1.Color;
            t1.SetColor(t2.Color);
            t2.SetColor(color1);
        }

    }
}