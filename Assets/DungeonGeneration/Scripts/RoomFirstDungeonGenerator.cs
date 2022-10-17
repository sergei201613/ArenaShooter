using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Sgorey.DungeonGeneration
{
    public class RoomFirstDungeonGenerator : DungeonGenerator
    {
        public Vector2Int DungeonSize
        {
            get
            {
                return new Vector2Int(_dungeonWidth, _dungeonHeight);
            }
            set
            {
                _dungeonWidth = value.x;
                _dungeonHeight = value.y;
            }
        }
        [SerializeField]
        private int _minRoomWidth = 4;
        [SerializeField]
        private int _minRoomHeight = 4;
        [SerializeField]
        private int _dungeonWidth = 50;
        [SerializeField]
        private int _dungeonHeight = 50;
        [SerializeField]
        [Range(0, 10)]
        private int _offset = 1;

        private Room _initialRoom;

        protected override HashSet<Room> GenerateRooms(Vector2Int start)
        {
            Vector3Int dungeonSize = new(_dungeonWidth, _dungeonHeight, 0);
            BoundsInt dungeonBounds = new((Vector3Int)startPosition, dungeonSize);

            var boundsList = PGAlgorithms.Bsp(dungeonBounds, _minRoomWidth, _minRoomHeight);

            HashSet<Room> rooms = new();
            foreach (var bounds in boundsList)
            {
                HashSet<Vector2Int> roomFloor = new();
                for (int col = _offset; col < bounds.size.x - _offset; col++)
                {
                    for (int raw = _offset; raw < bounds.size.y - _offset; raw++)
                    {
                        var offset = new Vector2Int(col, raw);
                        var pos = (Vector2Int)bounds.min + offset;
                        roomFloor.Add(pos);
                    }
                }
                var roomPos = (Vector2Int)Vector3Int.RoundToInt(bounds.center);
                Room room = new(roomPos, roomFloor);
                rooms.Add(room);
            }
            SetRoomTypes(rooms);
            return rooms;
        }

        private void SetRoomTypes(IReadOnlyCollection<Room> rooms)
        {
            List<Room> candidates = new(rooms);

            if (candidates.Count > 0)
                candidates.RemoveAt(SetInitialRoom(candidates));

            if (candidates.Count > 0)
                candidates.RemoveAt(SetFinishRoom(candidates));

            if (candidates.Count > 0)
                candidates.RemoveAt(SetBossRoom(candidates));
        }

        private int SetFinishRoom(IReadOnlyList<Room> candidates)
        {
            int idx = FindFarthestRoomTo(_initialRoom, candidates);
            candidates[idx].Type = RoomType.Finish;
            return idx;
        }

        private int SetBossRoom(IReadOnlyList<Room> candidates)
        {
            int idx = Random.Range(0, candidates.Count);
            candidates[idx].Type = RoomType.Boss;

            return idx;
        }

        private int SetInitialRoom(IReadOnlyList<Room> candidates)
        {
            _initialRoom = candidates[0];
            _initialRoom.Type = RoomType.Initial;
            return 0;
        }

        private HashSet<Vector2Int> GenerateCorridorFloor(in Room source,
            in Room destination)
        {
            HashSet<Vector2Int> corridor = new();
            var pos = source.Position;
            corridor.Add(pos);

            while (pos.y != destination.Position.y)
            {
                var yDiff = destination.Position.y - pos.y;
                var yDiffNormalized = yDiff / Mathf.Abs(yDiff);
                pos += new Vector2Int(0, yDiffNormalized);

                corridor.Add(pos);
            }

            while (pos.x != destination.Position.x)
            {
                var xDiff = destination.Position.x - pos.x;
                var xDiffNormalized = xDiff / Mathf.Abs(xDiff);
                pos += new Vector2Int(xDiffNormalized, 0);

                corridor.Add(pos);
            }
            return corridor;
        }

        private Room FindClosestRoomTo(Room room, 
            IReadOnlyCollection<Room> rooms)
        {
            Room closestRoom = rooms.First();
            float minDist = float.MaxValue;

            foreach (var currRoom in rooms)
            {
                if (currRoom == room)
                    continue;

                var pos = currRoom.Position;
                float currDist = Vector2.Distance(pos, room.Position);

                if (currDist < minDist)
                {
                    minDist = currDist;
                    closestRoom = currRoom;
                }
            }
            return closestRoom;
        }

        private int FindFarthestRoomTo(Room room, 
            IReadOnlyList<Room> rooms)
        {
            int farthestRoomIdx = 0;
            float maxDist = float.MinValue;

            for (int i = 0; i < rooms.Count; i++)
            {
                Room currRoom = rooms[i];
                var pos = currRoom.Position;
                float currDist = Vector2.Distance(pos, room.Position);

                if (currDist > maxDist)
                {
                    maxDist = currDist;
                    farthestRoomIdx = i;
                }
            }
            return farthestRoomIdx;
        }

        protected override HashSet<Corridor> GenerateCorridors(IReadOnlyCollection<Room> rooms)
        {
            HashSet<Corridor> corridors = new();
            List<Room> roomsTmp = new(rooms);

            var index = Random.Range(0, roomsTmp.Count);
            var currRoom = roomsTmp[index];

            while (roomsTmp.Count > 0)
            {
                var closestRoom = FindClosestRoomTo(currRoom, roomsTmp);

                // TODO: If we remove room from list, next FindClosestRoomTo
                // will ignore it, it can cause bugs, corridor can go through
                // room, and door positions will not detect properly.
                roomsTmp.Remove(closestRoom);

                var corridorFloor = GenerateCorridorFloor(currRoom, 
                    closestRoom);

                corridorFloor.ExceptWith(currRoom.FloorPositions);
                corridorFloor.ExceptWith(closestRoom.FloorPositions);

                currRoom = closestRoom;
                Corridor corridor = new(currRoom.Position, corridorFloor);
                corridors.Add(corridor);
            }
            return corridors;
        }
    }
}
