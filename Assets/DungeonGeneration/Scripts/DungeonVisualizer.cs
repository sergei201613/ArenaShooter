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
        protected GameObject floorPrefabDoor;

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
            {
                foreach (var pos in corridor.FloorPositions)
                {
                    if (pos == corridor.DoorPositionA || pos == corridor.DoorPositionB)
                        SpawnElement(floorPrefabDoor, pos);
                    else
                        SpawnElement(floorPrefabCorridor, pos);
                }
            }

            var floor = GetFloorPositions(dungeon);
            var directions = Vector2IntHelper.CardinalDirections;
            var wallPositions = GetWallPositions(floor, directions);
            SpawnElements(wallPositions, wallPrefab);
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
