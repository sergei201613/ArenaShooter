using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Sgorey.DungeonGeneration
{
    public class RandomWalkDungeonGenerator : DungeonGenerator
    {
        [SerializeField]
        protected RandomWalkParameters parameters;

        private int _iterations;
        private int _walkLength;
        private bool _startIterationFromRandomPosition;

        public override void Generate()
        {
            InitializeParameters();
            base.Generate();
        }

        private void InitializeParameters()
        {
            _iterations = parameters.Iterations;
            _walkLength = parameters.WalkLength;
            _startIterationFromRandomPosition =
                parameters.StartIterationFromRandomPosition;
        }

        public override HashSet<Vector2Int> GenerateFloorPositions(Vector2Int 
            startPosition)
        {
            var currentPos = startPosition;
            var floorPositions = new HashSet<Vector2Int>();

            for (int i = 0; i < _iterations; i++)
            {
                var path = PGAlgorithms.RandomWalk(currentPos, _walkLength);

                floorPositions.UnionWith(path);

                if (_startIterationFromRandomPosition)
                {
                    int index = Random.Range(0, floorPositions.Count);
                    currentPos = floorPositions.ElementAt(index);
                }
            }

            return floorPositions;
        }
    }
}
