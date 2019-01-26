using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeBar : MonoBehaviour {
    private float remainingTime;
    private float maxTime;
    private float originalWidth;

    private Room currentRoom;

    private RectTransform bar;

    private bool running;

    private void Awake() {
        bar = GetComponent<RectTransform>();
        originalWidth = bar.sizeDelta.x;
        running = false;
    }

    private void Start() {
        gameObject.SetActive(false);
    }

    public void StartRunning(Room room) {
        currentRoom = room;
        remainingTime = room.roomTime;
        maxTime = room.roomTime;
        running = true;
    }

    private void Update() {
        if (running) {
            remainingTime -= Time.deltaTime;
            bar.sizeDelta = new Vector2(originalWidth * remainingTime / maxTime, bar.sizeDelta.y);

            if (remainingTime <= 0) {
                Finish();
            }
        }
    }

    private void Finish() {
        running = false;
        currentRoom.Enlighten();
        bar.sizeDelta = new Vector2(originalWidth, bar.sizeDelta.y);
        gameObject.SetActive(false);
    }
}
