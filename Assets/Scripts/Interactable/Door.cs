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
    public AudioClip closedDoorHit;

    [Header("Door Initialization")]
    public bool closedDoor = false;

    private AudioSource audioSource;
    private SpriteRenderer spriteRenderer;

    private bool fadingIn;
    private bool fadingOut;
    private bool enabledInteraction;
    private float alpha;
    private float fadingSpeed = 1;

    public bool EnabledInteraction {
        get {
            return closedDoor ? false : enabledInteraction;
        }

        set {
            enabledInteraction = value;
        }
    }

    private void Awake() {
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        enabledInteraction = true;
    }

    public void Interact() { }

    public void ShowInteractable() {
        if (EnabledInteraction) {
            spriteRenderer.enabled = false;
            doorCollider.enabled = false;
            audioSource.PlayOneShot(openDoor);
        }
    }

    public void HideInteractable() {
        if (EnabledInteraction) {
            spriteRenderer.enabled = true;
            doorCollider.enabled = true;
            audioSource.PlayOneShot(closeDoor);
        }
    }

    public void PlayClosedDoorHit() {
        if (!EnabledInteraction) {
                audioSource.PlayOneShot(closedDoorHit);
            }
    }

    public void FadeIn() {
        fadingOut = false;
        fadingIn = true;
    }

    public void FadeOut() {
        fadingOut = true;
        fadingIn = false;
    }

    protected virtual void Update() {
        if (fadingIn) {
            alpha = Mathf.Clamp(alpha + fadingSpeed * Time.deltaTime, 0, 1);

            spriteRenderer.color = new Color(1, 1, 1, alpha);

            if (alpha == 1) {
                fadingIn = false;
            }
        } else if (fadingOut) {
            alpha = Mathf.Clamp(alpha - fadingSpeed * Time.deltaTime, 0, 1);

            spriteRenderer.color = new Color(1, 1, 1, alpha);

            if (alpha == 0) {
                fadingOut = false;
            }
        }
    }
}
