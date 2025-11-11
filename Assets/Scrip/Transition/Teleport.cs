using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace YFarm.Transition
{
    public class Teleport : MonoBehaviour
    {
        public string targetScene;
        public Vector3 transitionPoint;

        void OnTriggerEnter2D(Collider2D collision)
        {
            EventHandler.CallTransitionEvent(targetScene, transitionPoint);
        }
    }
}
