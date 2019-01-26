using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour {
    public SpriteRenderer lightRoomSprite;

    public float fadingSpeed = 1;

    private bool enlighted;
    private bool fading;

    private float alpha;

    private void Awake() {
        enlighted = false;
        fading = false;

        alpha = 0;

        lightRoomSprite.color = new Color(1, 1, 1, 0);
    }

    private void Update() {
        if (fading) {
            alpha = Mathf.Clamp(alpha + fadingSpeed * Time.deltaTime, 0, 1);
            lightRoomSprite.color = new Color(1, 1, 1, alpha);

            if (alpha == 1) {
                fading = false;
            }
        }
    }

    private void Enlighten() {
        enlighted = true;
        fading = true;
    }
}
