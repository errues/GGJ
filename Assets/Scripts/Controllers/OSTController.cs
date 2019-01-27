using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OSTController : MonoBehaviour {

    public AudioClip lightTheme;
    public AudioClip darkTheme;

    public AudioClip victoryClip;
    public AudioClip loseClip;

    [Range(0f,10f)]
    public float waitForVictoryLose = 5f;

    private bool inFight;
    private bool blockAudio;
    private bool playHappy;
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
        playHappy = true;
    }

    public void PlayLightTheme(bool noTime = false) {
        playHappy = true;
        if (!inFight && !blockAudio) {
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
        playHappy = false;
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
        StopAllCoroutines();
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
        playHappy = true;

        yield return new WaitForSeconds(6f);

        blockAudio = false;
        if (playHappy) {
            PlayLightTheme(true);
        }
        else {
            PlayDarkTheme(true);
        }
        audioSource.time = themeTime;
    }
}
