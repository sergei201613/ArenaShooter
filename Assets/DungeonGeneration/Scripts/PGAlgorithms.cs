using System.Collections.Generic;
using UnityEngine;

namespace Sgorey.DungeonGeneration
{
    /// <summary>
    /// ProceduralGenerationAlgorithms.
    /// </summary>
    public static class PGAlgorithms
    {
        public static HashSet<Vector2Int> RandomWalk(Vector2Int start, 
            int length)
        {
            var path = new HashSet<Vector2Int> { start };
            var prevPos = start;

            for (int i = 0; i < length; i++)
            {
                var offset = Vector2IntHelper.RandomCardinalDirection();
                var newPos = prevPos + offset;

                path.Add(newPos);
                prevPos = newPos;
            }
            return path;
        }

        public static List<Vector2Int> RandomWalkCorridor(
            Vector2Int start, int length, bool wide = false)
        {
            var dir = Vector2IntHelper.RandomCardinalDirection();
            var corridor = new List<Vector2Int>() { start };
            var currentPos = start;

            for (int i = 0; i < length; i++)
            {
                currentPos += dir;
                corridor.Add(currentPos);

                if (wide)
                {
                    var offset = new Vector2Int(dir.y, dir.x);
                    corridor.Add(currentPos + offset);
                }
            }
            return corridor;
        }

        /// <summary>
        /// Binary space partitioning.
        /// </summary>
        public static List<BoundsInt> Bsp(BoundsInt spaceToSplit, 
            int minWidth, int minHeight)
        {
            var roomsToSplit = new Queue<BoundsInt>();
            var splittedRooms = new List<BoundsInt>();
            roomsToSplit.Enqueue(spaceToSplit);
            while (roomsToSplit.Count > 0)
            {
                var room = roomsToSplit.Dequeue();
                if (room.size.y >= minHeight && room.size.x >= minWidth)
                {
                    if (Random.value < .5f)
                    {
                        if (room.size.y >= minHeight * 2)
                            SplitHorizontally(minHeight, roomsToSplit, room);
                        else if (room.size.x >= minWidth * 2)
                            SplitVertically(minWidth, roomsToSplit, room);
                        else
                            splittedRooms.Add(room);
                    }
                    else
                    {
                        if (room.size.x >= minWidth * 2)
                            SplitVertically(minWidth, roomsToSplit, room);
                        else if (room.size.y >= minHeight * 2)
                            SplitHorizontally(minHeight, roomsToSplit, room);
                        else
                            splittedRooms.Add(room);
                    }
                }
            }
            return splittedRooms;
        }

        private static void SplitVertically(int minWidth, 
            Queue<BoundsInt> roomsQueue, BoundsInt room)
        {
            var sizeX = room.size.x;
            var sizeY = room.size.y;
            var sizeZ = room.size.z;

            var xSplit = Random.Range(1, sizeX);
            var room1Size = new Vector3Int(xSplit, sizeY, sizeZ);
            BoundsInt room1 = new(room.min, room1Size);

            var room2Pos = new Vector3Int(room.min.x + xSplit, room.min.y, 
                room.min.z);
            var room2Size = new Vector3Int(sizeX - xSplit, sizeY, sizeZ);
            BoundsInt room2 = new(room2Pos, room2Size);

            roomsQueue.Enqueue(room1);
            roomsQueue.Enqueue(room2);
        }

        private static void SplitHorizontally(int minHeight, 
            Queue<BoundsInt> roomsQueue, BoundsInt room)
        {
            var sizeX = room.size.x;
            var sizeY = room.size.y;
            var sizeZ = room.size.z;

            var ySplit = Random.Range(1, sizeY);
            var room1Size = new Vector3Int(sizeX, ySplit, sizeZ);
            BoundsInt room1 = new(room.min, room1Size);

            var room2Pos = new Vector3Int(room.min.x, room.min.y + ySplit, 
                room.min.z);
            var room2Size = new Vector3Int(sizeX, sizeY - ySplit, sizeZ);
            BoundsInt room2 = new(room2Pos, room2Size);

            roomsQueue.Enqueue(room1);
            roomsQueue.Enqueue(room2);
        }
    }

    // TODO: Write custom Vector2 int class
    public static class Vector2IntHelper
    {
        public static IReadOnlyCollection<Vector2Int> CardinalDirections
        {
            get => _cardinalDirections;
        }

        public static Vector3 DungeonToWorldPosition(Vector2Int position, 
            float height, float scale = 1)
        {
            return new(
                position.x * scale,
                height,
                position.y * scale);
        }

        public static Vector2Int RandomCardinalDirection()
        {
            int rand = Random.Range(0, 4);
            return _cardinalDirections[rand];
        }

        private static readonly Vector2Int[] _cardinalDirections = 
        {
            Vector2Int.up,
            Vector2Int.right,
            Vector2Int.down,
            Vector2Int.left
        };
    }
}
