using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Sgorey.DungeonGeneration
{
    public class RoomFirstDungeonGenerator : DungeonGenerator
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
        private int _minBossRoomCount = 1;
        [SerializeField]
        private int _maxBossRoomCount = 3;
        [SerializeField]
        [Range(0, 10)]
        private int _offset = 1;
        [SerializeField]
        private GameObject[] _enemyPrefabs;
        [SerializeField]
        private GameObject[] _bossPrefabs;
        [SerializeField]
        private GameObject[] _lootPrefabs;
        [SerializeField]
        private GameObject[] _initialRoomLootPrefabs;
        [SerializeField]
        private GameObject _finishThingPrefab;
        [SerializeField]
        private NavMeshSurface _navMeshSurface;

        private Room _initialRoom;

        public override HashSet<Room> GenerateRooms(Vector2Int start)
        {
            Vector3Int size = new(_dungeonWidth, _dungeonHeight, 0);
            BoundsInt bounds = new((Vector3Int)startPosition, size);

            var boundsList = PGAlgorithms.Bsp(bounds, _minRoomWidth,
                _minRoomHeight);

            HashSet<Room> rooms = CreateRooms(boundsList);
            SetRoomTypes(rooms);

            return rooms;
        }

        private void SetRoomTypes(IReadOnlyCollection<Room> rooms)
        {
            List<Room> candidates = new(rooms);

            candidates.RemoveAt(SetInitialRoom(candidates));
            candidates.RemoveAt(SetFinishRoom(candidates));

            List<int> bossRoomIndexes = SetBossRooms(candidates);
            for (int i = 0; i < bossRoomIndexes.Count; i++)
                candidates.RemoveAt(bossRoomIndexes[i]);
        }

        private int SetFinishRoom(IReadOnlyList<Room> candidates)
        {
            int idx = FindFarthestRoomTo(_initialRoom, candidates);
            candidates[idx].Type = RoomType.Finish;
            return idx;
        }

        private List<int> SetBossRooms(IReadOnlyList<Room> candidates)
        {
            int count = Random.Range(_minBossRoomCount, _maxBossRoomCount + 1);

            List<int> candidateIndexes = new(candidates.Count);
            for (int i = 0; i < candidates.Count; i++)
                candidateIndexes.Add(i);

            List<int> bossRoomIndexes = new(count);

            for (int i = 0; i < Mathf.Min(count, candidateIndexes.Count); i++)
            {
                int idx = Random.Range(0, candidateIndexes.Count);
                bossRoomIndexes.Add(candidateIndexes[idx]);
                candidates[candidateIndexes[idx]].Type = RoomType.Boss;
                candidateIndexes.RemoveAt(idx);
            }

            return bossRoomIndexes;
        }

        private int SetInitialRoom(IReadOnlyList<Room> candidates)
        {
            _initialRoom = candidates[0];
            _initialRoom.Type = RoomType.Initial;
            return 0;
        }

        protected override Vector3 GetPlayerSpawnPosition()
        {
            return new Vector3(
                _initialRoom.Position.x * scale,
                height,
                _initialRoom.Position.y * scale);
        }

        protected override void SpawnEnemies(IReadOnlyCollection<Room> rooms)
        {
            _navMeshSurface.BuildNavMesh();

            foreach (var room in rooms)
            {
                if (room.Type == RoomType.Initial)
                    continue;

                Vector3 pos = DungeonToWorldPosition(room.Position);

                int index;
                GameObject prefab;

                if (room.Type == RoomType.Boss)
                {
                    index = Random.Range(0, _bossPrefabs.Length);
                    prefab = _bossPrefabs[index];
                }
                else
                {
                    index = Random.Range(0, _enemyPrefabs.Length);
                    prefab = _enemyPrefabs[index];
                }

                Instantiate(prefab, pos, Quaternion.identity, transform);
            }
        }

        protected override void SpawnLoot(IReadOnlyCollection<Room> rooms)
        {
            foreach (var room in rooms)
            {
                if (room.Type == RoomType.Finish)
                {
                    if (_finishThingPrefab != null)
                    {
                        Vector3 pos = DungeonToWorldPosition(room.Position);
                        Instantiate(_finishThingPrefab, pos, Quaternion.identity, 
                            transform);
                    }
                }
                else if (room.Type == RoomType.Initial)
                {
                    if (_initialRoomLootPrefabs.Length > 0)
                    {
                        Vector3 pos = DungeonToWorldPosition(room.RandomPosition);
                        int idx = Random.Range(0, _initialRoomLootPrefabs.Length);
                        var prefab = _initialRoomLootPrefabs[idx];
                        Instantiate(prefab, pos, Quaternion.identity, transform);
                    }
                }
            }
        }

        private Vector3 DungeonToWorldPosition(Vector2Int position)
        {
            return Vector2IntHelper.DungeonToWorldPosition(position, height, scale);
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

        private HashSet<Room> CreateRooms(IReadOnlyCollection<BoundsInt> boundsList)
        {
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
            return rooms;
        }

        public override HashSet<Corridor> GenerateCorridors(IReadOnlyCollection<Room> rooms)
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
