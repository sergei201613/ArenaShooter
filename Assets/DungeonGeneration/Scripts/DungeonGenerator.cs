using System.Collections.Generic;
using UnityEngine;

namespace Sgorey.DungeonGeneration
{
    [DefaultExecutionOrder(-100)]
    public abstract class DungeonGenerator : MonoBehaviour
    {
        [SerializeField]
        protected Vector2Int startPosition;

        protected abstract HashSet<Room> GenerateRooms(Vector2Int start);

        protected abstract HashSet<Corridor> GenerateCorridors(IReadOnlyCollection<Room> rooms);

        public virtual Dungeon Generate()
        {
            HashSet<Room> rooms = GenerateRooms(startPosition);
            HashSet<Corridor> corridors = GenerateCorridors(rooms);

            Dungeon dungeon = new(rooms, corridors);
            return dungeon;
        }
    }
}
