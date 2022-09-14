using System.Collections.Generic;
using UnityEngine;

namespace Sgorey.DungeonGeneration
{
    public class Room : Structure
    {
        public Room(Vector2Int position, HashSet<Vector2Int> floorPositions) 
            : base(position, floorPositions)
        {
        }
    }
}