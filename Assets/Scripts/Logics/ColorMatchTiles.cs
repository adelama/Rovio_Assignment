using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Rovio.TapMatch.Logic
{
    public class ColorMatchTiles
    {
        private HashSet<int> tileIndices;

        public int[] TileIndices => tileIndices.ToArray();

        public ColorMatchTiles()
        {
            tileIndices = new HashSet<int>();
        }

        public void Clear()
        {
            tileIndices.Clear();
        }

        public bool HasTile(int tileIndex)
        {
            return tileIndices.Contains(tileIndex);
        }

        public void AddTile(int tileIndex)
        {
            tileIndices.Add(tileIndex);
        }
    }
}
