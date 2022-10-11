using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Sgorey.DungeonGeneration
{
    public class Corridor : Structure
    {
        public Vector2Int DoorPositionA { get; private set; }

        public Vector2Int DoorPositionB { get; private set; }

        public Corridor(Vector2Int position, HashSet<Vector2Int> 
            floorPositions) : base(position, floorPositions)
        {
            DoorPositionA = floorPositions.First();
            DoorPositionB = floorPositions.Last();
        }
    }
}
