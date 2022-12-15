using UnityEngine;

namespace Sgorey.ArenaShooter.Editor
{
    public class ReplaceObjects : MonoBehaviour
    {
        public Transform Parent;
        public GameObject Prefab;

        [ContextMenu("Replace")]
        public void Replace()
        {
            while (Parent.childCount != 0)
            {
                Transform obj = Parent.GetChild(0);

                Vector3 pos = obj.position;
                Quaternion rot = obj.rotation;

                Instantiate(Prefab, pos, rot);
                DestroyImmediate(obj.gameObject);
            }
        }
    }
}
