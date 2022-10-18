using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Unity.FPS.Game
{
    public class ActorsManager : MonoBehaviour
    {
        public List<Actor> Actors { get; private set; }
        public GameObject Player { get; private set; }

        public void SetPlayer(GameObject player) => Player = player;

        void Awake()
        {
            Actors = new List<Actor>();

            // TODO: Bad
            var player = GameObject.FindWithTag("Player");
            Assert.IsNotNull(player);

            SetPlayer(player);

            var playerActor = player.GetComponent<Actor>();
            Assert.IsNotNull(playerActor);

            Actors.Add(playerActor);
        }
    }
}
