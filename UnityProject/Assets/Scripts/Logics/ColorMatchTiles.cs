using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Rovio.TapMatch.Logic
{
    public class ColorMatchTiles
    {
        private class TileIndexComparer : EqualityComparer<Tile>
        {
            public override bool Equals(Tile t1, Tile t2)
            {
                return t1.Index.Equals(t2.Index);
            }
            public override int GetHashCode(Tile t)
            {
                return t.Index;
            }
        }

        private HashSet<Tile> tiles;

        public Tile[] TilesArray => tiles.ToArray();
        public Tile[] TilesSortedArray => tiles.OrderBy(t=>t.Index).ToArray();

        public ColorMatchTiles()
        {
            tiles = new HashSet<Tile>(new TileIndexComparer());
        }

        public void Clear()
        {
            tiles.Clear();
        }

        public bool Contains(Tile tile)
        {
            return tiles.Contains(tile);
        }

        public void AddTile(Tile tile)
        {
            tiles.Add(tile);
        }
    }

}
