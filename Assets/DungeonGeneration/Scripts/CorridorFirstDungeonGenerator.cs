using System.Collections.Generic;
using UnityEngine;

namespace Sgorey.DungeonGeneration
{
    public class CorridorFirstDungeonGenerator : RandomWalkDungeonGenerator
    {
        [SerializeField]
        private int _corridorLength = 12;
        [SerializeField]
        private int _corridorCount = 6;
        [SerializeField]
        [Range(.1f, 1)]
        private float _roomPercent = .8f;
        [SerializeField]
        private bool _wideCorridors;

        public override HashSet<Vector2Int> GenerateFloorPositions()
        {
            var floorPositions = new HashSet<Vector2Int>();
            var currentPos = startPosition;

            for (int i = 0; i < _corridorCount; i++)
            {
                var corridor = PGAlgorithms.RandomWalkCorridor(currentPos, 
                    _corridorLength, _wideCorridors);

                currentPos = corridor[^1];
                floorPositions.UnionWith(corridor);
            }

            return floorPositions;
        }
    }
}
