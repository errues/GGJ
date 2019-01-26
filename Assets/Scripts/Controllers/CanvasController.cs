﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasController : MonoBehaviour {
    private TimeBar timeBar;

    private void Awake() {
        timeBar = GetComponentInChildren<TimeBar>(true);
    }
    
    public void StartTimeBar(CombatRoom room) {
        timeBar.StartRunning(room);
    }
}
