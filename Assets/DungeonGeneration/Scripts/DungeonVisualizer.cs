using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Sgorey.DungeonGeneration
{
    public class DungeonVisualizer : MonoBehaviour
    {
        [SerializeField]
        protected GameObject floorPrefabRoom;

        [SerializeField]
        protected GameObject floorPrefabCorridor;

        [SerializeField]
        protected GameObject doorPrefab;

        [SerializeField]
        protected GameObject wallPrefab;

        protected int scale;
        protected float height;

        public virtual void Visualize(Dungeon dungeon, int scale, float height)
        {
            this.scale = scale;
            this.height = height;

            foreach (var room in dungeon.Rooms)
                SpawnElements(room.FloorPositions, floorPrefabRoom);

            foreach (var corridor in dungeon.Corridors)
                SpawnElements(corridor.FloorPositions, floorPrefabCorridor);

            var floor = GetFloorPositions(dungeon);
            var directions = Vector2IntHelper.Directions;
            var wallPositions = GetWallPositions(floor, directions);
            SpawnElements(wallPositions, wallPrefab);

            foreach (var corridor in dungeon.Corridors)
            {
                foreach (var pos in corridor.FloorPositions)
                {
                    if (IsDoorPosition(pos, wallPositions))
                        SpawnElement(doorPrefab, pos);
                }
            }
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
            GameObject prefab)
        {
            foreach (var rawPos in positions)
                SpawnElement(prefab, rawPos);
        }

        private void SpawnElement(GameObject prefab, Vector2Int rawPos)
        {
            int x = rawPos.x * scale;
            int y = rawPos.y * scale;

            var position = new Vector3(x, height, y);
            Instantiate(prefab, position, Quaternion.identity, transform);
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
