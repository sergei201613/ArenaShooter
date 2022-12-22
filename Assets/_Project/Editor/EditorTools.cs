using Sgorey.Unity.Utils.Runtime;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Sgorey.ArenaShooter.Editor
{
    public class EditorTools
    {
        private const string DUNGEON_SCENE = "Assets/_Project/Common/Scenes/Dungeon.unity";
        private const string CASTLE_SCENE = "Assets/_Project/Common/Scenes/Castle.unity";

        private static string _lastEditScene;

        [MenuItem("Tools/Clear Player Prefs")]
        public static void ClearPlayerPrefs()
        {
            PlayerPrefs.DeleteAll();
        }

        [MenuItem("Open Scene/Castle")]
        public static void OpenCastleScene()
        {
            EditorSceneManager.OpenScene(CASTLE_SCENE);
        }

        [MenuItem("Open Scene/Dungeon")]
        public static void OpenDungeonScene()
        {
            EditorSceneManager.OpenScene(DUNGEON_SCENE);
        }

        [MenuItem("Open Scene/GameBoot")]
        public static void OpenGameBootScene()
        {
            EditorSceneManager.OpenScene(Boot.BOOT_SCENE_PATH);
        }

        [MenuItem("Play/Play")]
        public static void RunBootScene()
        {
            OpenBootSceneAndPlay();
        }

        [MenuItem("Play/Play Dungeon")]
        public static void RunDungeonScene()
        {
            OpenBootSceneAndPlay("Dungeon");
        }

        [MenuItem("Play/Play Castle")]
        public static void RunCastleScene()
        {
            OpenBootSceneAndPlay("Castle");
        }

        private static void OpenBootSceneAndPlay(string nextScene = null)
        {
            _lastEditScene = EditorSceneManager.GetActiveScene().path;
            string editScene = EditorSceneManager.GetActiveScene().name;

            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;

            EditorSceneManager.OpenScene(Boot.BOOT_SCENE_PATH);

            EditorApplication.EnterPlaymode();

            string sceneToPlay = string.IsNullOrEmpty(nextScene) ? 
                editScene : nextScene;

            Boot.SceneToLoad = sceneToPlay;
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
