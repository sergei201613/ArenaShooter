using UnityEngine;
using Sgorey.Unity.Utils.Runtime;

namespace Sgorey.ArenaShooter
{
    [RequireComponent(typeof(DistanceBasedOptimizer))]
    public class OptimizationController : MonoBehaviour
    {
        private DistanceBasedOptimizer _optimizer;
        private Camera _camera;

        private void Start()
        {
            _camera = Camera.main;
            _optimizer = gameObject.GetComp<DistanceBasedOptimizer>();
            var playerObj = this.FindComp<PlayerCharacter>();
            _optimizer.Init(playerObj.transform);

            Optimizable[] optimizables = FindObjectsOfType<Optimizable>();
            foreach (var opt in optimizables)
            {
                _optimizer.Register(opt);
            }
        }

        private void Update()
        {
            _optimizer.SetDistance(_camera.farClipPlane);
        }
    }
}
