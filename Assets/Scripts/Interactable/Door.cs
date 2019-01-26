using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource), typeof(Collider2D))]
public class Door : MonoBehaviour, Interactable {

    public AudioClip openDoor;
    public AudioClip closeDoor;

    private AudioSource audioSource;
    private SpriteRenderer spriteRenderer;
    private Collider2D doorCollider;

    public bool EnabledInteraction {
        get {
            return doorCollider.enabled;
        }

        set {
            doorCollider.enabled = value;
        }
    }

    private void Awake() {
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        doorCollider = GetComponent<Collider2D>();
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
