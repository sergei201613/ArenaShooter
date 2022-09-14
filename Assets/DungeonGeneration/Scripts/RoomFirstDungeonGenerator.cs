using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

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
        [SerializeField]
        private GameObject[] _enemyPrefabs;
        [SerializeField]
        private NavMeshSurface _navMeshSurface;

        private Vector3 _playerSpawnPos;
        private List<Room> _rooms = new(16);
        private List<Corridor> _corridors = new(16);

        public override HashSet<Vector2Int> GenerateFloor(Vector2Int start)
        {
            HashSet<Vector2Int> floorPositions = new();

            Vector3Int size = new(_dungeonWidth, _dungeonHeight, 0);
            BoundsInt bounds = new((Vector3Int)startPosition, size);

            var boundsList = PGAlgorithms.Bsp(bounds, _minRoomWidth, 
                _minRoomHeight);

            _rooms = CreateRooms(boundsList);
            foreach (var room in _rooms)
            {
                floorPositions.UnionWith(room.FloorPositions);
            }

            _playerSpawnPos.x = _rooms[0].Position.x * scale;
            _playerSpawnPos.y = height;
            _playerSpawnPos.z = _rooms[0].Position.y * scale;

            _corridors = GenerateCorridors(_rooms);
            foreach (var corridor in _corridors)
            {
                floorPositions.UnionWith(corridor.FloorPositions);
            }

            return floorPositions;
        }

        protected override Vector3 GetPlayerSpawnPosition()
        {
            return _playerSpawnPos;
        }

        protected override void SpawnEnemies()
        {
            _navMeshSurface.BuildNavMesh();

            foreach (var room in _rooms)
            {
                Vector3 pos = new(
                    room.Position.x * scale, 
                    height, 
                    room.Position.y * scale);

                int index = Random.Range(0, _enemyPrefabs.Length);
                var prefab = _enemyPrefabs[index];
                Instantiate(prefab, pos, Quaternion.identity, transform);
            }
        }

        private List<Corridor> GenerateCorridors(in List<Room> rooms)
        {
            List<Corridor> corridors = new();
            List<Room> roomsTmp = new(rooms);

            var index = Random.Range(0, roomsTmp.Count);
            var currRoom = roomsTmp[index];

            roomsTmp.RemoveAt(index);

            while (roomsTmp.Count > 0)
            {
                var closestRoom = FindClosestRoomTo(currRoom, roomsTmp);
                roomsTmp.Remove(closestRoom);

                var corridorFloor = GenerateCorridorFloor(currRoom, 
                    closestRoom);

                currRoom = closestRoom;
                Corridor corridor = new(currRoom.Position, corridorFloor);
                corridors.Add(corridor);
            }
            return corridors;
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
            in List<Room> rooms)
        {
            Room closestRoom = rooms[0];
            float minDist = float.MaxValue;

            foreach (var currRoom in rooms)
            {
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

        private List<Room> CreateRooms(List<BoundsInt> boundsList)
        {
            List<Room> rooms = new();
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
    }
}
