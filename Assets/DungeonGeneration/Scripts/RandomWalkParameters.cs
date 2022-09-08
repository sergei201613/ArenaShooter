using UnityEngine;

namespace Sgorey.DungeonGeneration
{
    [CreateAssetMenu(
        fileName = "RandomWalkGenerationParameters_", 
        menuName = "Dungeon Generation/Random Walk Generation Parameters")]
    public class RandomWalkParameters : ScriptableObject
    {
        public int Iterations = 10;
        public int WalkLength = 10;
        public bool StartIterationFromRandomPosition = true;
    }
}
