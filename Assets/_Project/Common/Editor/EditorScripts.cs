using Sgorey.Unity.Utils.Runtime;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Sgorey.ArenaShooter
{
    public class EditorScripts : MonoBehaviour
    {
        private const string BOOT_SCENE = "Assets/_Project/Common/Scenes/GameBoot.unity";

        private static string _lastEditScene;

        [MenuItem("Play/Play")]
        public static void RunBootScene()
        {
            OpenBootScene();
        }

        [MenuItem("Play/Play Dungeon")]
        public static void RunDungeonScene()
        {
            OpenBootScene("Dungeon");
        }

        [MenuItem("Play/Play Castle")]
        public static void RunCastleScene()
        {
            OpenBootScene("Castle");
        }

        private static void OpenBootScene(string nextScene = null)
        {
            _lastEditScene = EditorSceneManager.GetActiveScene().path;

            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;

            EditorSceneManager.OpenScene(BOOT_SCENE);

            if (!string.IsNullOrEmpty(nextScene))
            {
                FindObjectOfType<Boot>().SetScene(nextScene);
            }

            EditorApplication.EnterPlaymode();
        }

        private static void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.EnteredEditMode)
            {
                if (string.IsNullOrWhiteSpace(_lastEditScene))
                    return;

                EditorSceneManager.OpenScene(_lastEditScene);
            }
        }
    }
}
