using System;
using System.Collections.Generic;
using UnityEngine;

namespace Sgorey.ArenaShooter
{
    public class EnemyGroup : MonoBehaviour
    {
        [SerializeField]
        private List<EnemyController> _enemies = new();

        private void Awake()
        {
            if (_enemies.Count == 0)
                throw new Exception("There is no enemies in enemy group!");

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
