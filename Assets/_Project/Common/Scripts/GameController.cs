using Sgorey.Unity.Utils.Runtime;
using System.Collections;
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
            }

            _gameFlow = this.FindComp<GameFlowManager>();

            _gameFlow.LevelChanged += Initialize;

            MovePlayerToSpawnPoint();
        }

        private void MovePlayerToSpawnPoint()
        {
            Transform spawnPoint = GameObject
                .FindWithTag(PlayerSpawnPointTag).transform;

            _player.GetComponent<CharacterController>().enabled = false;

            StartCoroutine(Coroutine());

            IEnumerator Coroutine()
            {
                for (int i = 0; i < 5; i++)
                {
                    _player.transform.SetPositionAndRotation(
                        spawnPoint.position, spawnPoint.rotation);

                    yield return null;
                }
            }
            
            _player.GetComponent<CharacterController>().enabled = true;
        }

        private void OnDestroy()
        {
            _gameFlow.LevelChanged -= Initialize;
        }
    }
}
