using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Sgorey.DungeonGeneration
{
    // TODO: need refactoring
    public class RandomWalkDungeonGenerator : DungeonGenerator
    {
        [SerializeField]
        protected RandomWalkParameters parameters;

        private int _iterations;
        private int _walkLength;
        private bool _startIterationFromRandomPosition;

        public override Dungeon Generate()
        {
            InitializeParameters();
            return base.Generate();
        }

        private void InitializeParameters()
        {
            _iterations = parameters.Iterations;
            _walkLength = parameters.WalkLength;
            _startIterationFromRandomPosition =
                parameters.StartIterationFromRandomPosition;
        }

        public override HashSet<Room> GenerateRooms(Vector2Int start)
        {
            var currentPos = start;
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
            Room room = new(start, floorPositions);
            HashSet<Room> rooms = new() { room };
            return rooms;
        }

        public override HashSet<Corridor> GenerateCorridors(IReadOnlyCollection<Room> rooms)
        {
            throw new System.NotImplementedException();
        }
    }
}
