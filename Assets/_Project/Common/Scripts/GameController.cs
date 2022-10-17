using Sgorey.DungeonGeneration;
using Sgorey.Unity.Utils.Runtime;
using System;
using Unity.FPS.Game;
using UnityEngine;

namespace Sgorey.ArenaShooter
{
    // TODO: Bad
    [DefaultExecutionOrder(-100)]
    public class GameController : MonoBehaviour
    {
        [SerializeField] Boot _boot;

        public int Level { get; private set; } = 1;

        RoomFirstDungeonGenerator _generator;
        DungeonVisualizer _visualizer;

        private GameFlowManager _gameFlow;

        private void Awake()
        {
            _boot.BootComplete += Initialize;
        }

        private void Initialize()
        {
            _gameFlow = this.FindComp<GameFlowManager>();
            _generator = this.FindComp<RoomFirstDungeonGenerator>();
            _visualizer = this.FindComp<DungeonVisualizer>();

            int size = 40 + Level * 10;
            _generator.DungeonSize = new Vector2Int(size, size);

            var dungeon = _generator.Generate();
            _visualizer.Visualize(dungeon, 2, 10);

            _gameFlow.LevelPassed += ToNextLevel;
            _gameFlow.PlayerDied += ResetLevel;
            _gameFlow.LevelChanged += Initialize;
        }

        private void OnDestroy()
        {
            _gameFlow.LevelPassed -= ToNextLevel;
            _gameFlow.PlayerDied -= ResetLevel;
        }

        private void ToNextLevel() => Level++;

        private void ResetLevel() => Level = 1;
    }
}
