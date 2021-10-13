using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interaction
{
    [RequireComponent(typeof(Collider))]
    public class Interactable : MonoBehaviour
    {
        public PlayerInteraction.ObjectType ObjectType;

        private void OnTriggerEnter(Collider other) => Toggle(other, true);
        private void OnTriggerExit(Collider other) => Toggle(other, false);
        void Toggle(Collider other, bool touched)
        {
            if (other.gameObject.GetComponent<PlayerInteraction>() != null)
            {
                Debug.Log($"Player is now {(touched ? "" : "un")}able to interact with {gameObject.name}");
                PlayerInteraction.OnTouchInteractable?.Invoke(this, touched);
            }
        }
    }
}