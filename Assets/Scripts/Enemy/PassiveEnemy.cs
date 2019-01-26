using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PassiveEnemy : MonoBehaviour {
    public Transform lightSpriteTransform;
    public Transform darkSpriteTransform;

    private SpriteRenderer[] lightSpriteRenderers;
    private SpriteRenderer[] darkSpriteRenderers;

    private Collider2D[] colliders;

    private ExplorationRoom assignedRoom;

    private bool completed;
    private bool fadingIn;
    private bool fadingOut;
    private float fadingSpeed;
    private float alpha;

    private void Awake() {
        completed = false;

        lightSpriteRenderers = lightSpriteTransform.GetComponentsInChildren<SpriteRenderer>();
        darkSpriteRenderers = darkSpriteTransform.GetComponentsInChildren<SpriteRenderer>();

        foreach (SpriteRenderer sr in lightSpriteRenderers) {
            sr.color = new Color(1, 1, 1, 0);
        }

        fadingIn = false;
        fadingOut = false;
        alpha = 0;

        colliders = GetComponentsInChildren<Collider2D>();
    }

    public void AssignRoom(ExplorationRoom room) {
        assignedRoom = room;
        fadingSpeed = room.fadeSpeed;
    }
    
    public void Complete() {
        completed = true;
    }

    private void Update() {
        if (fadingIn) {
            alpha = Mathf.Clamp(alpha + fadingSpeed * Time.deltaTime, 0, 1);

            if (completed) {
                foreach (SpriteRenderer sr in lightSpriteRenderers) {
                    sr.color = new Color(1, 1, 1, alpha);
                }
            } else {
                foreach (SpriteRenderer sr in darkSpriteRenderers) {
                    sr.color = new Color(1, 1, 1, alpha);
                }
            }

            if (alpha == 1) {
                fadingIn = false;
                foreach (Collider2D c2d in colliders) {
                    c2d.enabled = true;
                }
            }
        } else if (fadingOut) {
            alpha = Mathf.Clamp(alpha - fadingSpeed * Time.deltaTime, 0, 1);

            if (completed) {
                foreach (SpriteRenderer sr in lightSpriteRenderers) {
                    sr.color = new Color(1, 1, 1, alpha);
                }
            } else {
                foreach (SpriteRenderer sr in darkSpriteRenderers) {
                    sr.color = new Color(1, 1, 1, alpha);
                }
            }

            if (alpha == 0) {
                fadingOut = false;
            }
        }
    }

    public void FadeIn() {
        fadingOut = false;
        fadingIn = true;
    }

    public void FadeOut() {
        fadingOut = true;
        fadingIn = false;

        foreach (Collider2D c2d in colliders) {
            c2d.enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (!completed) {
            assignedRoom.StartRoom();
        }
    }
}
