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
        protected Vector2Int startPosition;
        [SerializeField]
        protected DungeonVisualizer _dungeonVisualizer;
        [SerializeField]
        protected int scale = 1;
        [SerializeField]
        protected int height = 15;

        protected virtual void Awake()
        {
            Dungeon dungeon = CreateDungeon();

            SpawnEnemies(dungeon.Rooms);
            SpawnLoot(dungeon.Rooms);
            SpawnPlayer();
        }

        public Dungeon CreateDungeon()
        {
            Dungeon dungeon = Generate();
            _dungeonVisualizer.Visualize(dungeon, scale, height);
            return dungeon;
        }

        public abstract HashSet<Room> GenerateRooms(Vector2Int start);

        public abstract HashSet<Corridor> GenerateCorridors(IReadOnlyCollection<Room> rooms);

        public virtual Dungeon Generate()
        {
            HashSet<Room> rooms = GenerateRooms(startPosition);
            HashSet<Corridor> corridors = GenerateCorridors(rooms);

            Dungeon dungeon = new(rooms, corridors);
            return dungeon;
        }

        public virtual void ClearImmediate()
        {
            var dungTransform = _dungeonVisualizer.transform;
            while (dungTransform.childCount != 0)
                DestroyImmediate(dungTransform.GetChild(0).gameObject);
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
    }
}
