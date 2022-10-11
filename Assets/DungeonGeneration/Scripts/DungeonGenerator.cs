using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Sgorey.DungeonGeneration
{
    [DefaultExecutionOrder(-100)]
    public abstract class DungeonGenerator : MonoBehaviour
    {
        // TODO: Move dungeon prefab fields to DungeonVisualizer
        [SerializeField]
        protected GameObject playerPrefab;
        [SerializeField]
        protected GameObject floorPrefab;
        [SerializeField]
        protected GameObject basicWallPrefab;
        [SerializeField]
        protected Vector2Int startPosition;
        [SerializeField]
        protected DungeonVisualizer _dungeonVisualizer;
        [SerializeField]
        protected int scale = 1;
        [SerializeField]
        protected int height = 15;

        protected virtual void Awake()
        {
            Dungeon dungeon = Generate();
            _dungeonVisualizer.Visualize(dungeon);

            SpawnEnemies(dungeon.Rooms);
            SpawnLoot(dungeon.Rooms);
            SpawnPlayer();
        }

        public abstract HashSet<Room> GenerateRooms(Vector2Int start);

        public abstract HashSet<Corridor> GenerateCorridors(IReadOnlyCollection<Room> rooms);

        public virtual Dungeon Generate()
        {
            HashSet<Room> rooms = GenerateRooms(startPosition);
            HashSet<Corridor> corridors = GenerateCorridors(rooms);

            Dungeon dungeon = new(rooms, corridors);
            return dungeon;

            // TODO: Move to DungeonVisualizer
            //var wallPositions = GenerateWallPositions(floorPositions);
            //SpawnElements(wallPositions, basicWallPrefab);
        }

        public virtual HashSet<Vector2Int> GenerateWallPositions(IReadOnlyCollection<Vector2Int> floorPositions)
        {
            return FindWalls(floorPositions, Vector2IntHelper.CardinalDirections);
        }

        public virtual void ClearImmediate()
        {
            while (transform.childCount != 0)
                DestroyImmediate(transform.GetChild(0).gameObject);
        }

        public virtual void Clear()
        {
            for (int i = 0; i < transform.childCount; i++)
                Destroy(transform.GetChild(i).gameObject);
        }

        protected virtual void SpawnPlayer()
        {
            Vector3 playerPos = GetPlayerSpawnPosition();
            Instantiate(playerPrefab, playerPos, Quaternion.identity);
        }

        protected virtual void SpawnEnemies(IReadOnlyCollection<Room> rooms) { }

        protected virtual void SpawnLoot(IReadOnlyCollection<Room> rooms) { }

        protected virtual Vector3 GetPlayerSpawnPosition()
        {
            return new Vector3(startPosition.x, height,
                startPosition.y);
        }

        private void SpawnElements(IReadOnlyCollection<Vector2Int> positions, 
            GameObject prefab)
        {
            foreach (var rawPos in positions)
            {
                int x = rawPos.x * scale;
                int y = rawPos.y * scale;

                var position = new Vector3(x, height, y);
                Instantiate(prefab, position, Quaternion.identity, transform);
            }
        }

        private HashSet<Vector2Int> FindWalls(
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
    }
}
