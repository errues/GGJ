using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour {
    public float fadingSpeed = 1;
    public GameObject pausePanel;
    public Button continueButton;

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
    
    //Fade In-Out
    public void FadeIn() {
        fadeImage.FadeIn(fadingSpeed);
    }

    public void FadeOut() {
        fadeImage.FadeOut(fadingSpeed);
    }

    //Time Bar
    public void StartTimeBar(CombatRoom room) {
        timeBar.StartRunning(room);
    }

    public void TerminateTimeBar() {
        timeBar.Terminate();
    }


    public void PauseBar() {
        timeBar.Pause();
    }

    //Gestión del menu de pausa
    private void ShowPausePanel() {
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
        continueButton.Select();
    }

    public void HidePausePanel() {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void ExitGame() {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void Update() {
        if (Input.GetButtonUp("Pause")) {
            ShowPausePanel();
        }
    }
}
