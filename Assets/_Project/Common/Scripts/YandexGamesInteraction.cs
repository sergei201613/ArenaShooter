using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace TeaGames.ArenaShooter
{
    public class YandexGamesInteraction : MonoBehaviour
    {
        public bool IsWebGL => Application.platform == RuntimePlatform.WebGLPlayer;

        public bool IsAdd;

        private void Awake()
        {
            Loaded();
        }

        private void Update()
        {
            print(IsAdd);
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
                showInterstitial();
            }
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
