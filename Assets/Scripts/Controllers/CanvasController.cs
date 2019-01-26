using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasController : MonoBehaviour {
    public float fadingSpeed = 1;

    public bool IsFadingIn {
        get {
            return fadeImage.IsFadingIn;
        }
    }

    public bool IsFadingOut {
        get {
            return fadeImage.IsFadingOut;
        }
    }

    private TimeBar timeBar;
    private FadeImage fadeImage;

    private void Awake() {
        timeBar = GetComponentInChildren<TimeBar>();
        fadeImage = GetComponentInChildren<FadeImage>();
    }
    
    public void StartTimeBar(CombatRoom room) {
        timeBar.StartRunning(room);
    }

    public void TerminateTimeBar() {
        timeBar.Terminate();
    }

    public void FadeIn() {
        fadeImage.FadeIn(fadingSpeed);
    }

    public void FadeOut() {
        fadeImage.FadeOut(fadingSpeed);
    }
}
