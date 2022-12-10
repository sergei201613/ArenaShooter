using Sgorey.Unity.Utils.Runtime;
using UnityEngine;

namespace Sgorey.ArenaShooter
{
    [DefaultExecutionOrder(-100)]
    public class GameController : MonoBehaviour
    {
        public static bool InitStarted = false;

        [SerializeField] Boot _boot;
        [SerializeField] GameObject _player;

        private GameFlowManager _gameFlow;
        private const string PlayerSpawnPointTag = "PlayerSpawnPoint";

        private void Awake()
        {
            InitStarted = true;
            _boot.SceneLoaded += Initialize;
        }

        private void Initialize()
        {
            if (_gameFlow)
            {
                _gameFlow.LevelChanged -= Initialize;
                _gameFlow.LevelChanging -= MovePlayerToSpawnPoint;
            }

            _gameFlow = this.FindComp<GameFlowManager>();

            _gameFlow.LevelChanged += Initialize;
            _gameFlow.LevelChanging += MovePlayerToSpawnPoint;

            MovePlayerToSpawnPoint();
        }

        private void MovePlayerToSpawnPoint()
        {
            Transform spawnPoint = GameObject
                .FindWithTag(PlayerSpawnPointTag).transform;

            _player.transform.SetPositionAndRotation(
                spawnPoint.position, spawnPoint.rotation);
        }

        private void OnDestroy()
        {
            _gameFlow.LevelChanged -= Initialize;
        }
    }
}
