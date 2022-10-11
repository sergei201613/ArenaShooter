using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

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

        // TODO:
        //public override HashSet<Vector2Int> GenerateFloor(Vector2Int
        //    startPosition)
        //{
        //    var floor = new HashSet<Vector2Int>();

        //    var corridors = GenerateCorridors(out var potentialRooms);
        //    var rooms = GenerateRooms(potentialRooms);

        //    floor.UnionWith(corridors);
        //    floor.UnionWith(rooms);

        //    List<Vector2Int> deadEnds = FindDeadEnds(floor);
        //    var deadEndRooms = GenerateDeadEndRooms(deadEnds, rooms);

        //    floor.UnionWith(deadEndRooms);
        //    return floor;
        //}

        // TODO:
        //private HashSet<Vector2Int> GenerateDeadEndRooms(
        //    in List<Vector2Int> deadEnds, in HashSet<Vector2Int> rooms)
        //{
        //    foreach (var deadEnd in deadEnds)
        //    {
        //        if (!rooms.Contains(deadEnd))
        //        {
        //            var roomFloor = base.GenerateFloor(deadEnd);
        //            rooms.UnionWith(roomFloor);
        //        }
        //    }
        //    return rooms;
        //}

        private List<Vector2Int> FindDeadEnds(in HashSet<Vector2Int> positions)
        {
            var deadEnds = new List<Vector2Int>();
            foreach (var position in positions)
            {
                int neighborCount = 0;
                foreach (var dir in Vector2IntHelper.CardinalDirections)
                {
                    if (positions.Contains(position + dir))
                        neighborCount++;
                }
                if (neighborCount == 1)
                    deadEnds.Add(position);
            }
            return deadEnds;
        }

        // TODO:
        //private HashSet<Vector2Int> GenerateRooms(in IEnumerable<Vector2Int> 
        //    potentialRooms)
        //{
        //    var rooms = new HashSet<Vector2Int>();
        //    foreach (var roomPosition in potentialRooms)
        //    {
        //        if (Random.Range(0f, 1) <= _roomPercent)
        //        {
        //            var roomFloor = base.GenerateFloor(roomPosition);
        //            rooms.UnionWith(roomFloor);
        //        }
        //    }
        //    return rooms;
        //}

        private IEnumerable<Vector2Int> GenerateCorridors(out HashSet<Vector2Int> 
            potentialRooms)
        {
            var currentPos = startPosition;
            var floorPositions = new HashSet<Vector2Int>();

            potentialRooms = new HashSet<Vector2Int> { currentPos };

            for (int i = 0; i < _corridorCount; i++)
            {
                var corridor = PGAlgorithms.RandomWalkCorridor(currentPos,
                    _corridorLength, _wideCorridors);

                currentPos = corridor[^1];
                potentialRooms.Add(currentPos);
                floorPositions.UnionWith(corridor);
            }
            return floorPositions;
        }
    }
}
