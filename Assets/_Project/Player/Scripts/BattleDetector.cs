using UnityEngine;

namespace Sgorey.ArenaShooter
{
    public class BattleDetector : MonoBehaviour
    {
        public event System.Action BattleBegin;
        public event System.Action BattleOver;

        [SerializeField]
        private DetectionModule _detectionModule;

        [SerializeField]
        private Actor _actor;

        [SerializeField]
        private Collider[] _selfColliders;

        private void OnEnable()
        {
            _detectionModule.TargetDetected += OnTargetDetected;
            _detectionModule.TargetLost += OnTargetLost;
        }

        private void OnDisable()
        {
            _detectionModule.TargetDetected -= OnTargetDetected;
            _detectionModule.TargetLost -= OnTargetLost;
        }

        private void Update()
        {
            _detectionModule.HandleTargetDetection(_actor, _selfColliders);
        }

        private void OnTargetDetected(GameObject target)
        {
            BattleBegin?.Invoke();
        }

        private void OnTargetLost()
        {
            BattleOver?.Invoke();
        }
    }
}
