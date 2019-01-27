using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OSTController : MonoBehaviour {

    public AudioClip lightTheme;
    public AudioClip darkTheme;

    public AudioClip victoryClip;
    public AudioClip loseClip;

    private bool inFight;
    private bool blockAudio;
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
        blockAudio = false;
    }

    public void PlayLightTheme(bool noTime = false) {
        if(!inFight && !blockAudio) {
            audioSource.loop = true;
            if (noTime) {
                audioSource.time = 0f;
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
        if (!inFight && !blockAudio) {
            audioSource.loop = true;
            if (noTime) {
                audioSource.time = 0f;
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
        audioSource.loop = true;
        inFight = true;
        audioSource.time = 0f;
        audioSource.clip = fightTheme;
        audioSource.Play();
    }

    public void FinishVictoryFightTheme() {
        audioSource.loop = false;
        inFight = false;
        audioSource.time = 0f;
        audioSource.clip = victoryClip;
        audioSource.Play();

        StartCoroutine(FinishFightTheme());
    }

    public void FinishLoseFightTheme() {
        audioSource.loop = false;
        inFight = false;
        audioSource.time = 0f;
        audioSource.clip = loseClip;
        audioSource.Play();

        StartCoroutine(FinishFightTheme());
    }

    private IEnumerator FinishFightTheme() {
        blockAudio = true;
        yield return new WaitForSeconds(6f);
        blockAudio = false;
        PlayLightTheme(true);
        audioSource.time = themeTime;
    }
}
