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
            foreach (GameObject obj in Parent)
            {
                Vector3 pos = obj.transform.position;
                Quaternion rot = obj.transform.rotation;

                Instantiate(Prefab, pos, rot);
                Destroy(obj);
            }
        }
    }
}
