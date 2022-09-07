using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Sgorey.DungeonGeneration
{
    public class RandomWalkDungeonGenerator : MonoBehaviour
    {
        [SerializeField]
        private GameObject _floorPrefab;
        [SerializeField]
        private int _positionsMultiplier = 1;
        [SerializeField]
        private Vector2Int _startPosition;
        [SerializeField]
        private int _iterations = 10;
        [SerializeField]
        private int _walkLength = 10;
        [SerializeField]
        private bool _startRandomlyEachIteration = true;

        private void Awake()
        {
            Generate();
        }

        // TODO: To base class
        public void Generate()
        {
            var floorPositions = RunRandomWalk();

            foreach (var rawPosition in floorPositions)
            {
                int x = rawPosition.x * _positionsMultiplier;
                int z = rawPosition.y * _positionsMultiplier;

                var position = new Vector3(x, 15, z);

                Instantiate(_floorPrefab, position, Quaternion.identity, transform);
            }
        }

        private HashSet<Vector2Int> RunRandomWalk()
        {
            var currentPos = _startPosition;
            var floorPositions = new HashSet<Vector2Int>();

            for (int i = 0; i < _iterations; i++)
            {
                var path = ProceduralGenerationAlgorithms
                    .RandomWalk(currentPos, _walkLength);

                floorPositions.UnionWith(path);

                if (_startRandomlyEachIteration)
                {
                    int index = Random.Range(0, floorPositions.Count);
                    currentPos = floorPositions.ElementAt(index);
                }
            }

            return floorPositions;
        }
    }
}
