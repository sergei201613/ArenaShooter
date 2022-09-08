using System.Collections.Generic;
using UnityEngine;

namespace Sgorey.DungeonGeneration
{
    /// <summary>
    /// ProceduralGenerationAlgorithms.
    /// </summary>
    public static class PGAlgorithms
    {
        public static HashSet<Vector2Int> RandomWalk(Vector2Int start, 
            int length)
        {
            var path = new HashSet<Vector2Int> { start };
            var prevPos = start;

            for (int i = 0; i < length; i++)
            {
                var offset = Vector2IntHelper.RandomCardinalDirection();
                var newPos = prevPos + offset;

                path.Add(newPos);
                prevPos = newPos;
            }
            return path;
        }

        public static List<Vector2Int> RandomWalkCorridor(
            Vector2Int start, int length, bool wide = false)
        {
            var dir = Vector2IntHelper.RandomCardinalDirection();
            var corridor = new List<Vector2Int>() { start };
            var currentPos = start;

            for (int i = 0; i < length; i++)
            {
                currentPos += dir;
                corridor.Add(currentPos);

                if (wide)
                {
                    var offset = new Vector2Int(dir.y, dir.x);
                    corridor.Add(currentPos + offset);
                }
            }
            return corridor;
        }
    }

    public static class Vector2IntHelper
    {
        public static IEnumerable<Vector2Int> CardinalDirections
        {
            get => _cardinalDirections;
        }

        private static readonly Vector2Int[] _cardinalDirections = 
        {
            Vector2Int.up,
            Vector2Int.right,
            Vector2Int.down,
            Vector2Int.left
        };

        public static Vector2Int RandomCardinalDirection()
        {
            int rand = Random.Range(0, 4);
            return _cardinalDirections[rand];
        }
    }
}
