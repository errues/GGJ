using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInteractor : MonoBehaviour {

    HashSet<Interactable> triggeredInteractables;

    private void Awake() {
        triggeredInteractables = new HashSet<Interactable>();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        Interactable interactable = collision.gameObject.GetComponent<Interactable>();
        if (interactable != null) {
            triggeredInteractables.Add(interactable);
            interactable.ShowInteractable();
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        Interactable interactable = collision.gameObject.GetComponent<Interactable>();
        if (triggeredInteractables.Contains(interactable)) {
            triggeredInteractables.Remove(interactable);
            interactable.HideInteractable();
        }
    }
}
