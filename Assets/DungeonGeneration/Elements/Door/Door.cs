using UnityEngine;

namespace Sgorey.ArenaShooter
{
    public class Door : MonoBehaviour
    {
        [SerializeField]
        private float _targetPositionYOpen;

        [SerializeField]
        private float _lerpSpeed = 5f;

        private float _targetPositionYClose;
        private float _targetPositionY;

        private void Awake()
        {
            _targetPositionYClose = transform.position.y;
            _targetPositionY = _targetPositionYClose;
        }

        private void Update()
        {
            transform.position = new(
                transform.position.x, 
                Mathf.Lerp(transform.position.y, _targetPositionY, _lerpSpeed * Time.deltaTime),
                transform.position.z);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<PlayerCharacter>(out var player))
            {
                _targetPositionY = _targetPositionYOpen;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent<PlayerCharacter>(out var player))
            {
                _targetPositionY = _targetPositionYClose;
            }
        }
    }
}
