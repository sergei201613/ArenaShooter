using UnityEngine;
using UnityEngine.SceneManagement;

namespace Sgorey.ArenaShooter
{
    public class LoadFromBootScene : MonoBehaviour
    {
        private void Awake()
        {
            if (!GameController.InitStarted)
            {
                SceneManager.LoadScene("GameBoot");
            }
            Destroy(gameObject);
        }
    }
}
