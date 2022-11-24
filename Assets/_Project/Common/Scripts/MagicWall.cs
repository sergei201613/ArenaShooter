using UnityEngine;

namespace Sgorey.ArenaShooter
{
    public class MagicWall : MonoBehaviour
    {
        [SerializeField]
        private GameObject _wallObject;

        [SerializeField]
        private ParticleSystem _particles;

        private BattleDetector _battleDetector;

        private void Awake()
        {
            _battleDetector = FindObjectOfType<BattleDetector>();
        }

        private void OnEnable()
        {
            _battleDetector.BattleBegin += Activate;
            _battleDetector.BattleOver += Deactivate;
        }

        private void OnDisable()
        {
            _battleDetector.BattleBegin -= Activate;
            _battleDetector.BattleOver -= Deactivate;
        }

        private void Activate()
        {
            _particles.Play();
            _wallObject.SetActive(true);
        }

        private void Deactivate()
        {
            _particles.Stop();
            _wallObject.SetActive(false);
        }
    }
}
