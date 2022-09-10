using System.Collections;
using System.Collections.Generic;

namespace Rovio.TapMatch.Logic
{
    public static class LogicConstants
    {
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
