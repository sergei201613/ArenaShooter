using Sgorey.DungeonGeneration;
using Sgorey.Unity.Utils.Runtime;
using System;
using System.Collections;
using Unity.FPS.Game;
using UnityEngine;

namespace Sgorey.ArenaShooter
{
    // TODO: Bad
    [DefaultExecutionOrder(-100)]
    public class GameController : MonoBehaviour
    {
        [SerializeField] Boot _boot;
        [SerializeField] GameObject _player;

        public int Level { get; private set; } = 1;

        RoomFirstDungeonGenerator _generator;
        DungeonVisualizer _visualizer;

        private GameFlowManager _gameFlow;

        private void Awake()
        {
            _boot.SceneLoaded += Initialize;
        }

        private void Initialize()
        {
            _gameFlow = this.FindComp<GameFlowManager>();
            _generator = this.FindComp<RoomFirstDungeonGenerator>();
            _visualizer = this.FindComp<DungeonVisualizer>();

            _gameFlow.LevelPassed += ToNextLevel;
            _gameFlow.PlayerDied += ResetLevel;
            _gameFlow.LevelChanged += Initialize;
            _visualizer.EnemySpawned += ProcessEnemy;

            _generator.DungeonSize = GetDungeonSizeByLevel(Level);

            var dungeon = _generator.Generate();
            _visualizer.Visualize(dungeon, 2, 10);

            var pos = _visualizer.GetPlayerSpawnPoint();
            StartCoroutine(Delay());

            IEnumerator Delay()
            {
                // TODO: shit code, i love this FPS template!
                yield return new WaitForEndOfFrame();
                _player.transform.position = pos;
                yield return new WaitForEndOfFrame();
                _player.transform.position = pos;
                yield return new WaitForEndOfFrame();
                _player.transform.position = pos;
            }
        }

        private void OnDestroy()
        {
            _gameFlow.LevelPassed -= ToNextLevel;
            _gameFlow.PlayerDied -= ResetLevel;
            _gameFlow.LevelChanged -= Initialize;
            _visualizer.EnemySpawned -= ProcessEnemy;
        }

        private Vector2Int GetDungeonSizeByLevel(int level)
        {
            int size = 40 + Level * 10;
            return new Vector2Int(size, size);
        }

        private void ProcessEnemy(GameObject enemy)
        {
            var health = enemy.GetComp<Health>();
            health.MaxHealth *= Level * (3f/2) / 3f;
        }

        private void ToNextLevel() => Level++;

        private void ResetLevel() => Level = 1;
    }
}
