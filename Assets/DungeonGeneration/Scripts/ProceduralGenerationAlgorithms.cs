using System.Collections.Generic;
using UnityEngine;

namespace Sgorey.DungeonGeneration
{
    public static class ProceduralGenerationAlgorithms
    {
        public static HashSet<Vector2Int> RandomWalk(Vector2Int startPosition, int length)
        {
            var path = new HashSet<Vector2Int>
            {
                startPosition
            };

            var prevPos = startPosition;

            for (int i = 0; i < length; i++)
            {
                var offset = RandomCardinalDirection();
                var newPos = prevPos + offset;

                path.Add(newPos);
                prevPos = newPos;
            }

            return path;
        }

        private static Vector2Int RandomCardinalDirection()
        {
            int rand = Random.Range(0, 4);

            return rand switch
            {
                0 => Vector2Int.up,
                1 => Vector2Int.right,
                2 => Vector2Int.down,
                3 => Vector2Int.left,
                _ => Vector2Int.up
            };
        }
    }
}
