using UnityEngine;
using Sgorey.Unity.Utils.Runtime;

namespace Sgorey.ArenaShooter
{
    [RequireComponent(typeof(DistanceBasedOptimizer))]
    public class OptimizationController : MonoBehaviour
    {
        private void Start()
        {
            var optimizer = gameObject.GetComp<DistanceBasedOptimizer>();
            var playerObj = this.FindComp<PlayerCharacter>().gameObject;
            optimizer.Init(playerObj);
        }
    }
}
