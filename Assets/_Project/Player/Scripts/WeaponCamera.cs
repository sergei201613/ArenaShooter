using UnityEngine;

namespace Sgorey.ArenaShooter
{
    public class WeaponCamera : MonoBehaviour
    {
        [SerializeField]
        private float _offsetSpeed = 5f;
        [SerializeField]
        private float _offsetMlt = 2f;

        private float _offsetX = 0;
        private float _offsetY = 0;

        private void LateUpdate()
        {
            float targetX = Input.GetAxis("Mouse X");
            float targetY = Input.GetAxis("Mouse Y");

            float dt = Time.deltaTime;

            _offsetX = Mathf.Lerp(_offsetX, targetX, dt * _offsetSpeed);
            _offsetY = Mathf.Lerp(_offsetY, targetY, dt * _offsetSpeed);

            var offset = new Vector3(_offsetY, _offsetX, 0f) * _offsetMlt;
            transform.localEulerAngles = offset;
        }
    }
}
