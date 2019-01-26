using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, Interactable {

    public AudioClip openDoor;
    public AudioClip closeDoor;

    private AudioSource audioSource;
    private SpriteRenderer spriteRenderer;

    private void Awake() {
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Interact() { }

    public void ShowInteractable() {
        spriteRenderer.enabled = false;
        audioSource.PlayOneShot(openDoor);
    }

    public void HideInteractable() {
        spriteRenderer.enabled = true;
        audioSource.PlayOneShot(closeDoor);
    }
}
