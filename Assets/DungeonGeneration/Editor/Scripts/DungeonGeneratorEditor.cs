#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Sgorey.DungeonGeneration.Editor
{
    [CustomEditor(typeof(DungeonGenerator), true)]
    public class DungeonGeneratorEditor : UnityEditor.Editor
    {
        private DungeonGenerator _generator;

        private void Awake()
        {
            _generator = (DungeonGenerator)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Generate"))
            {
                // TODO:
                //ClearImmediate();
                //CreateDungeon();
            }

            if (GUILayout.Button("Clear"))
            {
                // TODO:
                //_generator.ClearImmediate();
            }
        }
    }
}
#endif
