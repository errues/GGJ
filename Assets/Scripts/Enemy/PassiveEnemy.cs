using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PassiveEnemy : MonoBehaviour {
    public Room assignedRoom;

    private CanvasController canvasController;

    private bool completed;

    private void Awake() {
        canvasController = GameObject.FindGameObjectWithTag("Canvas").GetComponent<CanvasController>();
        completed = false;
    }

    private void Start() {
        assignedRoom.AssignEnemy(this);
    }

    public void Complete() {
        completed = true;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (!completed) {
            // Hacer la transición a la habitación de combate y iniciar la barra de tiempo
        }
    }
}
