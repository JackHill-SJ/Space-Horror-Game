using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interaction
{
    [RequireComponent(typeof(Canvas))]
    public class InteractableWindow : MonoBehaviour
    {
        public static InteractableWindow Instance;
        List<Interactable> interactables = new List<Interactable>();
        bool touching;

        private void Awake()
        {
            if (Instance != null) Destroy(gameObject);
            Instance = this;

            PlayerInteraction.OnTouchInteractable += Touched;
        }
        private void OnDestroy()
        {
            if (Instance == this) Instance = null;
            interactables = null;

            PlayerInteraction.OnTouchInteractable -= Touched;
        }

        void Touched(Interactable interactable, bool touched)
        {
            if (touched)
            {
                interactables.Add(interactable);
                if (interactables.Count == 1) transform.GetChild((int)interactable.ObjectType).gameObject.SetActive(touched);
            }
            else
            {
                bool first = interactables.IndexOf(interactable) == 0;
                interactables.Remove(interactable);
                if (first)
                {
                    transform.GetChild((int)interactable.ObjectType).gameObject.SetActive(touched);
                    transform.GetChild((int)interactables[0].ObjectType).gameObject.SetActive(!touched);
                }
            }
        }
    }
}