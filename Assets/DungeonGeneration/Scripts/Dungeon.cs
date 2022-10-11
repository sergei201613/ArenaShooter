using System.Collections.Generic;

namespace Sgorey.DungeonGeneration
{
    public class Dungeon
    {
        public IReadOnlyCollection<Room> Rooms => _rooms;

        public IReadOnlyCollection<Corridor> Corridors => _corridors;

        private readonly HashSet<Room> _rooms;
        private readonly HashSet<Corridor> _corridors;

        public Dungeon(HashSet<Room> rooms, HashSet<Corridor> corridors)
        {
            _rooms = rooms;
            _corridors = corridors;
        }
    }
}
