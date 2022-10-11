using System.Collections.Generic;
using UnityEngine;

namespace Sgorey.DungeonGeneration
{
    public class Structure
    {
        public Vector2Int Position => _position;
        public IReadOnlyCollection<Vector2Int> FloorPositions => _floorPositions;
        // TODO: public IReadOnlyCollection<Vector2Int> WallPositions => _wallPositions;

        private Vector2Int _position;
        private readonly HashSet<Vector2Int> _floorPositions;
        // TODO: private readonly HashSet<Vector2Int> _wallPositions;

        public Structure(Vector2Int position, 
            HashSet<Vector2Int> floorPositions)
        {
            _position = position;
            _floorPositions = floorPositions;
        }
    }
}
