using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeImage : MonoBehaviour {
    private bool fadingIn;
    private bool fadingOut;
    private float alpha;

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

    private float fadingSpeed;
    private Image fadingImage;

    private void Awake() {
        fadingIn = false;
        fadingOut = false;
        alpha = 0;

        fadingImage = GetComponent<Image>();
    }

    private void Update() {
        if (fadingIn) {
            alpha = Mathf.Clamp(alpha + fadingSpeed * Time.deltaTime, 0, 1);

            fadingImage.color = new Color(0, 0, 0, alpha);
            
            if (alpha == 1) {
                fadingIn = false;
            }
        } else if (fadingOut) {
            alpha = Mathf.Clamp(alpha - fadingSpeed * Time.deltaTime, 0, 1);

            fadingImage.color = new Color(0, 0, 0, alpha);
            
            if (alpha == 0) {
                fadingOut = false;
            }
        }
    }

    public void FadeIn(float fadingSpeed) {
        fadingOut = false;
        fadingIn = true;

        this.fadingSpeed = fadingSpeed;
    }

    public void FadeOut(float fadingSpeed) {
        fadingOut = true;
        fadingIn = false;

        this.fadingSpeed = fadingSpeed;
    }
}
