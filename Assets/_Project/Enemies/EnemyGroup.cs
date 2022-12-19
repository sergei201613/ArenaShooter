using System.Collections.Generic;
using UnityEngine;

namespace Sgorey.ArenaShooter
{
    public class EnemyGroup : MonoBehaviour
    {
        private readonly List<EnemyController> _enemies = new();

        private void Awake()
        {
            foreach (Transform t in transform)
            {
                if (t.TryGetComponent<EnemyController>(out var e))
                {
                    _enemies.Add(e);
                }
            }

            if (_enemies.Count == 0)
            {
                Debug.LogError("There is no enemies in enemy group!", this);
            }

            foreach (EnemyController enemy in _enemies)
            {
                var detection = enemy.GetComponentInChildren<DetectionModule>();
                if (detection)
                {
                    detection.TargetDetected += OnTargetDetected;
                }
            }
        }

        private void OnTargetDetected(GameObject target)
        {
            foreach (EnemyController enemy in _enemies)
            {
                enemy.SetTarget(target);
            }
        }
    }
}
