using System.Collections.Generic;
using UnityEngine;

namespace Sgorey.DungeonGeneration
{
    [DefaultExecutionOrder(-100)]
    public abstract class DungeonGenerator : MonoBehaviour
    {
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

            // TODO: Move to DungeonFiller.cs
            SpawnEnemies(dungeon.Rooms);
            SpawnLoot(dungeon.Rooms);
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

        protected virtual void SpawnEnemies(IReadOnlyCollection<Room> rooms) { }

        protected virtual void SpawnLoot(IReadOnlyCollection<Room> rooms) { }
    }
}
