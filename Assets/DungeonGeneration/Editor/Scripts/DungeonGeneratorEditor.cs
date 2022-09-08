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
                _generator.ClearImmediate();
                _generator.Generate();
            }

            if (GUILayout.Button("Clear"))
                _generator.ClearImmediate();
        }
    }
}
