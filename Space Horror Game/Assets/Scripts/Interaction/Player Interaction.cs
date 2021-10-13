using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interaction
{
    public class PlayerInteraction : MonoBehaviour
    {
        public enum ObjectType { Door, Light, Collectable, Tool, Item }
        public static Action<Interactable, bool> OnTouchInteractable;
        public static Action<Interactable> OnInteract;

        public KeyCode InteractButton = KeyCode.E;

        Interactable i;
        bool canInteract;
        bool disabled = false;

        private void Awake()
        {
            OnTouchInteractable += StoreTouched;
            OnInteract += Interacting;
        }
        private void OnDestroy()
        {
            OnTouchInteractable -= StoreTouched;
            OnInteract -= Interacting;
        }
        private void Update()
        {
            if (canInteract && Input.GetKey(InteractButton) && !disabled) OnInteract?.Invoke(i);
        }
        void StoreTouched(Interactable interactable, bool touched)
        {
            if (i == null)
            {
                i = touched ? interactable : null;
                canInteract = touched;
            }
        }
        void Interacting(Interactable interactable)
        {
            Console.Log($"Interacting with {interactable.gameObject.name} of type {interactable.ObjectType}");
            switch (interactable.ObjectType)
            {
                case ObjectType.Door:
                    break;
                case ObjectType.Light:
                    break;
                case ObjectType.Collectable:
                    break;
                case ObjectType.Tool:
                    break;
                case ObjectType.Item:
                    break;
                default:
                    Console.Log($"Interaction with {interactable.ObjectType} is not implemented yet.");
                    break;
            }
        }
    }
}