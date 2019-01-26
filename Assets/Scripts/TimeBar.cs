using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeBar : MonoBehaviour {
    private float remainingTime;
    private float maxTime;
    private float originalWidth;

    private CombatRoom currentRoom;

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

    public void StartRunning(CombatRoom room) {
        currentRoom = room;
        remainingTime = room.GetRoomTime();
        maxTime = room.GetRoomTime();
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
        currentRoom.Finish();
        bar.sizeDelta = new Vector2(originalWidth, bar.sizeDelta.y);
        gameObject.SetActive(false);
    }
}
