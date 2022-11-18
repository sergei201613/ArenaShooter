using Sgorey.Unity.Utils.Runtime;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace Sgorey.DungeonGeneration
{
    public class DungeonVisualizer : MonoBehaviour
    {
        public event System.Action<GameObject> EnemySpawned;

        [Tooltip("Dungeon Elements")]
        [SerializeField] protected GameObject floorPrefabRoom;
        [SerializeField] protected GameObject ceilingPrefab;
        [SerializeField] protected GameObject floorPrefabCorridor;
        [SerializeField] protected GameObject doorPrefab;
        [SerializeField] protected GameObject wallPrefab;
        [SerializeField] protected GameObject roomCenterPropPrefab;
        [Tooltip("Dungeon Enemies")]
        [SerializeField] private GameObject[] _enemyPrefabs;
        [SerializeField] private GameObject[] _bossPrefabs;
        [SerializeField] private int _minEnemyCount = 2;
        [SerializeField] private int _maxEnemyCount = 4;
        [Tooltip("Dungeon Loot")]
        [SerializeField] private GameObject[] _lootPrefabs;
        [SerializeField] private GameObject[] _initialRoomLootPrefabs;
        [Tooltip("Other")]
        [SerializeField] private GameObject _finishThingPrefab;
        [SerializeField] private DistanceBasedOptimizer _optimizer;
        [SerializeField] private NavMeshSurface _navMeshSurface;
        [SerializeField] private float _ceilingYOffset = 2f;

        protected int scale;
        protected float height;
        private Dungeon _dungeon;

        public virtual void Visualize(Dungeon dungeon, int scale, float height)
        {
            this.scale = scale;
            this.height = height;

            _dungeon = dungeon;

            SpawnRoomFloor(dungeon);
            SpawnCorridorFloor(dungeon);
            DecorateRooms(dungeon.Rooms);

            HashSet<Vector2Int> wallPositions = SpawnWalls(dungeon);
            SpawnDoors(dungeon, wallPositions);

            SpawnEnemies(dungeon.Rooms);
            SpawnLoot(dungeon.Rooms);
        }

        public Vector3 GetPlayerSpawnPoint()
        {
            foreach (var room in _dungeon.Rooms)
            {
                if (room.Type == RoomType.Initial)
                {
                    return DungeonToWorldPosition(room.Position);
                }
            }
            throw new System.Exception("There is no initial room in the dungeon!");
        }

        protected virtual void SpawnEnemies(IReadOnlyCollection<Room> rooms)
        {
            _navMeshSurface.BuildNavMesh();

            foreach (var room in rooms)
            {
                if (room.Type == RoomType.Initial)
                    continue;

                int index;
                GameObject prefab;
                GameObject obj;

                // TODO: Code duplication
                if (room.Type == RoomType.Boss)
                {
                    if (_bossPrefabs.Count() == 0)
                        continue;

                    Vector3 pos = DungeonToWorldPosition(room.RandomPosition);
                    index = Random.Range(0, _bossPrefabs.Length);
                    prefab = _bossPrefabs[index];
                    obj = Instantiate(prefab, pos, Quaternion.identity, transform);

                    var opt = obj.AddComponent<Optimizable>();
                    _optimizer.Register(opt);

                    EnemySpawned?.Invoke(obj);
                }
                else
                {
                    int count = Random.Range(_minEnemyCount, _maxEnemyCount);
                    for (int i = 0; i < count; i++)
                    {
                        Vector3 pos = DungeonToWorldPosition(room.RandomPosition);
                        index = Random.Range(0, _enemyPrefabs.Length);
                        prefab = _enemyPrefabs[index];
                        obj = Instantiate(prefab, pos, Quaternion.identity, transform);

                        var opt = obj.AddComponent<Optimizable>();
                        _optimizer.Register(opt);

                        EnemySpawned?.Invoke(obj);
                    }
                }
            }
        }

        protected virtual void SpawnLoot(IReadOnlyCollection<Room> rooms)
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
                else if (room.Type == RoomType.None)
                {
                    if (_lootPrefabs.Length > 0)
                    {
                        Vector3 pos = DungeonToWorldPosition(room.RandomPosition);
                        int idx = Random.Range(0, _lootPrefabs.Length);
                        var prefab = _lootPrefabs[idx];
                        Instantiate(prefab, pos, Quaternion.identity, transform);
                    }
                }
            }
        }

        private void SpawnDoors(Dungeon dungeon, HashSet<Vector2Int> wallPositions)
        {
            foreach (var corridor in dungeon.Corridors)
            {
                foreach (var pos in corridor.FloorPositions)
                {
                    if (IsDoorPosition(pos, wallPositions))
                        SpawnElement(doorPrefab, pos, true, height);
                }
            }
        }

        private HashSet<Vector2Int> SpawnWalls(Dungeon dungeon)
        {
            var floor = GetFloorPositions(dungeon);
            var directions = Vector2IntHelper.Directions;
            var wallPositions = GetWallPositions(floor, directions);
            SpawnElements(wallPositions, wallPrefab, true, height);
            return wallPositions;
        }

        private void SpawnCorridorFloor(Dungeon dungeon)
        {
            foreach (var corridor in dungeon.Corridors)
                SpawnElements(corridor.FloorPositions, floorPrefabCorridor, 
                    true, height);

            foreach (var corridor in dungeon.Corridors)
                SpawnElements(corridor.FloorPositions, ceilingPrefab, 
                    true, height + _ceilingYOffset);
        }

        private void SpawnRoomFloor(Dungeon dungeon)
        {
            foreach (var room in dungeon.Rooms)
                SpawnElements(room.FloorPositions, floorPrefabRoom, 
                    true, height);

            foreach (var room in dungeon.Rooms)
                SpawnElements(room.FloorPositions, ceilingPrefab, true, 
                    height + _ceilingYOffset);
        }

        private static bool IsDoorPosition(Vector2Int pos, 
            IReadOnlyCollection<Vector2Int> wallPositions)
        {
            var neighbors = Vector2IntHelper.Neighbors(pos, wallPositions, 
                Vector2IntHelper.Directions);

            var upDownNeighbors = Vector2IntHelper.Neighbors(pos, 
                wallPositions, Vector2IntHelper.UpDownDirections);

            var leftRightNeighbors = Vector2IntHelper.Neighbors(pos, 
                wallPositions, Vector2IntHelper.LeftRightDirections);

            if (neighbors.Count >= 3 && neighbors.Count <= 5)
            {
                if (upDownNeighbors.Count == 2 && leftRightNeighbors.Count == 0)
                    return true;

                if (leftRightNeighbors.Count == 2 && upDownNeighbors.Count == 0)
                    return true;

                return false;
            }
            else
            {
                return false;
            }
        }

        private static HashSet<Vector2Int> GetFloorPositions(Dungeon dungeon)
        {
            HashSet<Vector2Int> floorPositions = new();

            foreach (var room in dungeon.Rooms)
                floorPositions.UnionWith(room.FloorPositions);

            foreach (var corridor in dungeon.Corridors)
                floorPositions.UnionWith(corridor.FloorPositions);

            return floorPositions;
        }

        private void SpawnElements(IReadOnlyCollection<Vector2Int> positions, 
            GameObject prefab, bool optimize, float height)
        {
            foreach (var rawPos in positions)
                SpawnElement(prefab, rawPos, optimize, height);
        }

        private void SpawnElement(GameObject prefab, Vector2Int rawPos, 
            bool optimize, float height)
        {
            // TODO: Vector2IntHelper knows about dungeon, maybe rename it to
            // DungeonHelper
            var pos = Vector2IntHelper.DungeonToWorldPosition(rawPos, height, scale);
            var obj = Instantiate(prefab, pos, Quaternion.identity, transform);

            if (optimize)
            {
                var opt = obj.AddComponent<Optimizable>();
                _optimizer.Register(opt);
            }
        }

        private HashSet<Vector2Int> GetWallPositions(
            IReadOnlyCollection<Vector2Int> floorPositions,
            IReadOnlyCollection<Vector2Int> directions)
        {
            var wallPositions = new HashSet<Vector2Int>();

            foreach (var position in floorPositions)
            {
                foreach (var direction in directions)
                {
                    var neighborPosition = position + direction;

                    if (!floorPositions.Contains(neighborPosition))
                        wallPositions.Add(neighborPosition);
                }
            }
            return wallPositions;
        }

        private Vector3 DungeonToWorldPosition(Vector2Int position)
        {
            return Vector2IntHelper.DungeonToWorldPosition(position, height, scale);
        }

        private void DecorateRooms(IReadOnlyCollection<Room> rooms)
        {
            foreach (var room in rooms)
            {
                if (room.Type == RoomType.Initial)
                    continue;

                if (room.Type == RoomType.Finish)
                    continue;

                SpawnElement(roomCenterPropPrefab, room.Position,
                    true, height);
            }
        }
    }
}
