using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkBarLogo : MonoBehaviour {
    public float rotationSpeed = 60f;

    private RectTransform rectTransform;

    private void Awake() {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update() {
        rectTransform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
    }
}
