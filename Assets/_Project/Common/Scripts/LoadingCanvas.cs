using Sgorey.Unity.Utils.Runtime;
using UnityEngine;

namespace Sgorey.ArenaShooter
{
    public class LoadingCanvas : MonoBehaviour
    {
        [SerializeField] private Boot _boot;
        [SerializeField] private GameObject _root;

        private GameFlowManager _gameFlow;

        private void Awake()
        {
            _boot.SceneLoaded += Initialize;
        }

        private void Initialize()
        {
            _boot.SceneLoaded -= Initialize;

            _gameFlow = this.FindComp<GameFlowManager>();
            _gameFlow.LevelChanged += OnLevelChanged;
            _gameFlow.LevelChanging += OnLevelChanging;

            _root.SetActive(false);
        }

        private void OnLevelChanging()
        {
            _gameFlow.LevelChanging -= OnLevelChanging;
            _root.SetActive(true);
        }

        private void OnLevelChanged()
        {
            _gameFlow.LevelChanged -= OnLevelChanged;
            _root.SetActive(false);
            Initialize();
        }
    }
}
