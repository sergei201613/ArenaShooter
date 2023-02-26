using System.Runtime.InteropServices;
using UnityEngine;

namespace TeaGames.ArenaShooter
{
    public class YandexGamesInteraction : MonoBehaviour
    {
        public bool IsWebGL => Application.platform == RuntimePlatform.WebGLPlayer;

        public bool IsAdd = false;
        public bool IsMobile = false;

        [SerializeField] private GameObject _mobileControlPanel;

        private void Awake()
        {
            Loaded();
        }

        private void Update()
        {
        }

        public void Loaded()
        {
            if (IsWebGL)
            {
                loaded();
            }
        }

        public void ShowInterstitial()
        {
            if (IsWebGL)
            {
                //showInterstitial();
            }
        }

        [ContextMenu(nameof(OnMobile))]
        public void OnMobile()
        {
            print("OnMobile (Unity)");
            IsMobile = true;
            _mobileControlPanel.SetActive(true);
        }

        [ContextMenu(nameof(OnInterstitialOpened))]
        public void OnInterstitialOpened()
        {
            print("OnInterstitialOpened (Unity)");
            IsAdd = true;
        }

        [ContextMenu(nameof(OnInterstitialClosed))]
        public void OnInterstitialClosed()
        {
            print("OnInterstitialClosed (Unity)");
            IsAdd = false;
        }

        [DllImport("__Internal")]
        private static extern void loaded();

        [DllImport("__Internal")]
        private static extern void showInterstitial();
    }
}
