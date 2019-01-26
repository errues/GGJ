using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Room : MonoBehaviour {
    private Collider2D[] colliders;

    protected bool fadingIn;
    protected bool fadingOut;
    protected float alpha;

    protected float fadingSpeed;

    public bool IsFadingIn {
        get {
            return fadingIn;
        }
    }

    public bool IsFadingOut {
        get {
            return fadingOut;
        }
    }

    protected virtual void Awake() {
        colliders = GetComponentsInChildren<Collider2D>();

        fadingIn = false;
        fadingOut = false;
        alpha = 1;
    }

    public void FadeIn() {
        fadingOut = false;
        fadingIn = true;

        foreach (Collider2D col in colliders) {
            col.enabled = true;
        }
    }

    public void FadeOut() {
        fadingOut = true;
        fadingIn = false;

        foreach (Collider2D col in colliders) {
            col.enabled = false;
        }
    }

    protected virtual void Update() {
        if (fadingIn) {
            alpha = Mathf.Clamp(alpha + fadingSpeed * Time.deltaTime, 0, 1);
        } else if (fadingOut) {
            alpha = Mathf.Clamp(alpha - fadingSpeed * Time.deltaTime, 0, 1);
        }
    }

    protected void CheckFadings() {
        if (fadingIn) {
            if (alpha == 1) {
                fadingIn = false;
            }
        } else if (fadingOut) {
            if (alpha == 0) {
                fadingOut = false;
            }
        }
    }
}
