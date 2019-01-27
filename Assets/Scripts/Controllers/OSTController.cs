using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OSTController : MonoBehaviour {

    public AudioClip lightTheme;
    public AudioClip darkTheme;

    public AudioClip victoryClip;
    public AudioClip loseClip;

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
                audioSource.Stop();
                audioSource.clip = lightTheme;
                audioSource.Play();
            }
            else {
                themeTime = audioSource.time;
                audioSource.Stop();
                audioSource.clip = lightTheme;
                audioSource.Play();
                audioSource.time = themeTime;
            }
        }
    }

    public void PlayDarkTheme(bool noTime = false) {
        if (!inFight) {
            if (noTime) {
                audioSource.Stop();
                audioSource.clip = darkTheme;
                audioSource.Play();
            }
            else {
                themeTime = audioSource.time;
                audioSource.Stop();
                audioSource.clip = darkTheme;
                audioSource.Play();
                audioSource.time = themeTime;
            }
        }
    }

    public void PlayFightTheme(AudioClip fightTheme) {
        inFight = true;
        audioSource.Stop();
        audioSource.clip = fightTheme;
        audioSource.Play();
    }

    public void FinishVictoryFightTheme() {
        inFight = false;
        audioSource.Stop();
        audioSource.clip = victoryClip;
        audioSource.Play();

        StartCoroutine(FinishFightTheme());
    }

    public void FinishLoseFightTheme() {
        inFight = false;
        audioSource.Stop();
        audioSource.clip = loseClip;
        audioSource.Play();

        StartCoroutine(FinishFightTheme());
    }

    private IEnumerator FinishFightTheme() {
        yield return new WaitForSeconds(5f);

        PlayLightTheme(true);
        audioSource.time = themeTime;
    }
}
