using Sgorey.Unity.Utils.Runtime;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Sgorey.DungeonGeneration
{
    public class DungeonVisualizer : MonoBehaviour
    {
        [SerializeField]
        protected GameObject playerPrefab;

        [SerializeField]
        protected GameObject floorPrefabRoom;

        [SerializeField]
        protected GameObject floorPrefabCorridor;

        [SerializeField]
        protected GameObject doorPrefab;

        [SerializeField]
        protected GameObject wallPrefab;

        [SerializeField]
        private DistanceBasedOptimizer _optimizer;

        protected int scale;
        protected float height;

        public virtual void Visualize(Dungeon dungeon, int scale, float height)
        {
            this.scale = scale;
            this.height = height;

            SpawnRoomFloor(dungeon);
            SpawnCorridorFloor(dungeon);

            HashSet<Vector2Int> wallPositions = SpawnWalls(dungeon);
            SpawnDoors(dungeon, wallPositions);

            SpawnPlayer(dungeon);
        }

        private void SpawnPlayer(Dungeon dungeon)
        {
            foreach (var room in dungeon.Rooms)
            {
                if (room.Type == RoomType.Initial)
                {
                    SpawnElement(playerPrefab, room.Position, false);
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
                        SpawnElement(doorPrefab, pos, true);
                }
            }
        }

        private HashSet<Vector2Int> SpawnWalls(Dungeon dungeon)
        {
            var floor = GetFloorPositions(dungeon);
            var directions = Vector2IntHelper.Directions;
            var wallPositions = GetWallPositions(floor, directions);
            SpawnElements(wallPositions, wallPrefab, true);
            return wallPositions;
        }

        private void SpawnCorridorFloor(Dungeon dungeon)
        {
            foreach (var corridor in dungeon.Corridors)
                SpawnElements(corridor.FloorPositions, floorPrefabCorridor, true);
        }

        private void SpawnRoomFloor(Dungeon dungeon)
        {
            foreach (var room in dungeon.Rooms)
                SpawnElements(room.FloorPositions, floorPrefabRoom, true);
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
            GameObject prefab, bool optimize)
        {
            foreach (var rawPos in positions)
                SpawnElement(prefab, rawPos, optimize);
        }

        private void SpawnElement(GameObject prefab, Vector2Int rawPos, bool optimize)
        {
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
    }
}
