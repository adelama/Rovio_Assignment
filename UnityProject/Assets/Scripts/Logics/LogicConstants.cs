using System.Collections;
using System.Collections.Generic;

namespace Rovio.TapMatch.Logic
{
    public static class LogicConstants
    {
        public const int MinColorsType = 3;
        public const int MaxColorsType = 6;
        public const int MinLevelWidth = 5;
        public const int MaxLevelWidth = 20;
        public const int MinLevelHeight = 5;
        public const int MaxLevelHeight = 20;

        public enum TileColor
        {
            Red = 0,
            Green = 1,
            Blue = 2,
            Yellow = 3,
            Purple = 4,
            Brown = 5,
            None = 6
        }

        public enum TileNeighbor
        {
            Top = 0,
            Bottom = 1,
            Left = 2,
            Right = 3,
            Any = 4
        }
    }
}
