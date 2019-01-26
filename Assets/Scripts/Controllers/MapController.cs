using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour {

    [Header("Map Size")]
    public Vector2 bounds = Vector2.one;


    private List<Door> doors;

    private void Awake() {
        doors = new List<Door>();

        foreach (Door door in GetComponentsInChildren<Door>()) {
            doors.Add(door);
        }
    }

    public void DissableDoors() {
        foreach (Door door in doors) {
            door.EnabledInteraction = false;
        }
    }

    public void EnableDoors() {
        foreach (Door door in doors) {
            door.EnabledInteraction = true;
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(transform.position, bounds);
    }
}
