using System.Collections.Generic;
using UnityEngine;

namespace Sgorey.DungeonGeneration
{
    [DefaultExecutionOrder(-100)]
    public abstract class DungeonGenerator : MonoBehaviour
    {
        [SerializeField]
        protected GameObject playerPrefab;
        [SerializeField]
        protected GameObject floorPrefab;
        [SerializeField]
        protected GameObject basicWallPrefab;
        [SerializeField]
        protected Vector2Int startPosition;
        [SerializeField]
        protected int scale = 1;
        [SerializeField]
        protected int height = 15;

        [SerializeField]
        private bool _generateOnAwake = true;

        protected virtual void Awake()
        {
            if (_generateOnAwake)
            {
                Generate();
                SpawnEnemies();
                SpawnLoot();
                SpawnPlayer();
            }
        }

        public abstract HashSet<Vector2Int> GenerateFloor(Vector2Int start);

        public virtual void Generate()
        {
            var floorPositions = GenerateFloor(startPosition);
            SpawnElements(floorPositions, floorPrefab);

            var wallPositions = GenerateWallPositions(floorPositions);
            SpawnElements(wallPositions, basicWallPrefab);
        }

        public virtual HashSet<Vector2Int> GenerateWallPositions(
            HashSet<Vector2Int> floorPositions)
        {
            return FindWalls(floorPositions, 
                Vector2IntHelper.CardinalDirections);
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

        protected virtual void SpawnEnemies() { }

        protected virtual void SpawnLoot() { }

        protected virtual Vector3 GetPlayerSpawnPosition()
        {
            return new Vector3(startPosition.x, height,
                startPosition.y);
        }

        private void SpawnElements(HashSet<Vector2Int> positions, 
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
            HashSet<Vector2Int> floorPositions,
            IEnumerable<Vector2Int> directions)
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
