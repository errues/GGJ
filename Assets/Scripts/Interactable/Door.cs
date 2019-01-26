using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Door : MonoBehaviour, Interactable {

    [Header("Door Colliders")]
    public Collider2D doorCollider;
    public Collider2D doorInteractionTrigger;

    [Header("Door Sounds")]
    public AudioClip openDoor;
    public AudioClip closeDoor;

    private AudioSource audioSource;
    private SpriteRenderer spriteRenderer;

    public bool EnabledInteraction {
        get {
            return doorInteractionTrigger.enabled;
        }

        set {
            doorInteractionTrigger.enabled = value;
        }
    }

    private void Awake() {
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Interact() { }

    public void ShowInteractable() {
        spriteRenderer.enabled = false;
        doorCollider.enabled = false;
        audioSource.PlayOneShot(openDoor);

    }

    public void HideInteractable() {
        spriteRenderer.enabled = true;
        doorCollider.enabled = true;
        audioSource.PlayOneShot(closeDoor);
    }
}
