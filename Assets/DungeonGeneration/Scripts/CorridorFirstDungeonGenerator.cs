using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Sgorey.DungeonGeneration
{
    public class CorridorFirstDungeonGenerator : RandomWalkDungeonGenerator
    {
        [SerializeField]
        private int _corridorLength = 12;
        [SerializeField]
        private int _corridorCount = 6;
        [SerializeField]
        [Range(.1f, 1)]
        private float _roomPercent = .8f;
        [SerializeField]
        private bool _wideCorridors;

        public override HashSet<Vector2Int> GenerateFloorPositions(Vector2Int
            startPosition)
        {
            var floorPositions = new HashSet<Vector2Int>();
            var potentialRoomPositions = new HashSet<Vector2Int>();

            GenerateCorridorPositions(floorPositions, potentialRoomPositions);
            var roomPositions = GenerateRoomPositions(potentialRoomPositions);

            floorPositions.UnionWith(roomPositions);
            return floorPositions;
        }

        private HashSet<Vector2Int> GenerateRoomPositions(HashSet<Vector2Int> 
            potentialRoomPositions)
        {
            var roomPositions = new HashSet<Vector2Int>();
            int roomCount = Mathf.RoundToInt(potentialRoomPositions.Count * 
                _roomPercent);

            List<Vector2Int> roomsToCreate = potentialRoomPositions
                .OrderBy(x => Guid.NewGuid())
                .Take(roomCount)
                .ToList();

            foreach (var roomPosition in roomsToCreate)
            {
                var roomFloor = base.GenerateFloorPositions(roomPosition);
                roomPositions.UnionWith(roomFloor);
            }

            return roomPositions;
        }

        private void GenerateCorridorPositions(HashSet<Vector2Int> floorPositions, HashSet<Vector2Int> potentialRoomPositions)
        {
            var currentPos = startPosition;
            potentialRoomPositions.Add(currentPos);

            for (int i = 0; i < _corridorCount; i++)
            {
                var corridor = PGAlgorithms.RandomWalkCorridor(currentPos,
                    _corridorLength, _wideCorridors);

                currentPos = corridor[^1];
                potentialRoomPositions.Add(currentPos);
                floorPositions.UnionWith(corridor);
            }
        }
    }
}
