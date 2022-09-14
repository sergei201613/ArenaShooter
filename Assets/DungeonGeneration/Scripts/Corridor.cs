using System.Collections.Generic;
using UnityEngine;

namespace Sgorey.DungeonGeneration
{
    public class Corridor : Structure
    {
        public Corridor(Vector2Int position, HashSet<Vector2Int> 
            floorPositions) : base(position, floorPositions)
        {
        }
    }
}
