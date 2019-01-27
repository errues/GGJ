using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathAnimation : MonoBehaviour {
    public GameObject good;
    public GameObject bad;

    public void GoGood() {
        good.SetActive(true);
        bad.SetActive(false);
    }

    public void GoBad() {
        good.SetActive(false);
        bad.SetActive(true);
    }
}
