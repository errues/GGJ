using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OSTController : MonoBehaviour {

    public AudioClip lightTheme;
    public AudioClip darkTheme;

    private bool inFight;
    private float themeTime;
    private AudioSource audioSource;

    public bool InFight {
        get {
            return inFight;
        }
    }

    private void Awake() {
        audioSource = GetComponent<AudioSource>();
        inFight = false;
    }

    public void PlayLightTheme(bool noTime = false) {
        if(!inFight) {
            if (noTime) {
                audioSource.clip = lightTheme;
                audioSource.Play();
            }
            else {
                themeTime = audioSource.time;
                audioSource.clip = lightTheme;
                audioSource.Play();
                audioSource.time = themeTime;
            }
        }
    }

    public void PlayDarkTheme(bool noTime = false) {
        if (!inFight) {
            if (noTime) {
                audioSource.clip = darkTheme;
                audioSource.Play();
            }
            else {
                themeTime = audioSource.time;
                audioSource.clip = darkTheme;
                audioSource.Play();
                audioSource.time = themeTime;
            }
        }
    }

    public void PlayFightTheme(AudioClip fightTheme) {
        inFight = true;
        audioSource.clip = fightTheme;
        audioSource.Play();
    }

    public void FinishFightTheme() {
        inFight = false;
        PlayLightTheme(true);
        audioSource.time = themeTime;
    }

    public void PlayDeathTheme() {

    } 

    public void PlayVictoryTheme() {

    }
}
