using System;
using System.Collections.Generic;
using UnityEngine;

namespace Sgorey.DungeonGeneration
{
    public class RoomFirstDungeonGenerator : RandomWalkDungeonGenerator
    {
        [SerializeField]
        private int _minRoomWidth = 4;
        [SerializeField]
        private int _minRoomHeight = 4;
        [SerializeField]
        private int _dungeonWidth = 20;
        [SerializeField]
        private int _dungeonHeight = 20;
        [SerializeField]
        [Range(0, 10)]
        private int _offset = 1;
        [SerializeField]
        private bool _randomWalkRooms = false;

        public override HashSet<Vector2Int> GenerateFloor(Vector2Int start)
        {
            Vector3Int size = new(_dungeonWidth, _dungeonHeight, 0);
            BoundsInt bounds = new((Vector3Int)startPosition, size);
            var rooms = PGAlgorithms.BinarySpacePartitioning(bounds, 
                _minRoomWidth, _minRoomHeight);

            HashSet<Vector2Int> floor = new();
            floor = CreateRooms(rooms);
            return floor;
        }

        private HashSet<Vector2Int> CreateRooms(List<BoundsInt> rooms)
        {
            HashSet<Vector2Int> floor = new();
            foreach (var room in rooms)
            {
                for (int col = _offset; col < room.size.x - _offset; col++)
                {
                    for (int raw = _offset; raw < room.size.y - _offset; raw++)
                    {
                        var offset = new Vector2Int(col, raw);
                        var pos = (Vector2Int)room.min + offset;
                        floor.Add(pos);
                    }
                }
            }
            return floor;
        }
    }
}
