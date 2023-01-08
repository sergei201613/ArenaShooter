using UnityEngine;

namespace Sgorey.ArenaShooter
{
    public class ResetAmbient : MonoBehaviour
    {
        private void Awake()
        {
            AudioUtility.SetMasterVolume(1f);
        }
    }
}
