using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour {
    public List<ExplorationRoom> explorationRooms;
    public List<CombatRoom> combatRooms;

    [Header("Map Size")]
    public Vector2 bounds = Vector2.one;

    private List<Door> doors;

    private void Awake() {
        doors = new List<Door>();

        foreach (Door door in GetComponentsInChildren<Door>()) {
            doors.Add(door);
        }
    }

    private void Start() {
        foreach(CombatRoom cr in combatRooms) {
            cr.Hide();
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

    public void FadeInMap() {
        foreach(Room r in explorationRooms) {
            r.FadeIn();
        }
    }

    public void FadeOutMap() {
        foreach (Room r in explorationRooms) {
            r.FadeOut();
        }
    }
}
