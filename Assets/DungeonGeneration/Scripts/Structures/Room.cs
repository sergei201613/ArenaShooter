using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Sgorey.DungeonGeneration
{
    public class Room : Structure
    {
        public RoomType Type { get; set; } = RoomType.None;

        public Vector2Int RandomPosition
        {
            get
            {
                int idx = Random.Range(0, FloorPositions.Count);
                return FloorPositions.ElementAt(idx);
            }
        }

        public Room(Vector2Int position, HashSet<Vector2Int> floorPositions) 
            : base(position, floorPositions)
        {
        }
    }
}
