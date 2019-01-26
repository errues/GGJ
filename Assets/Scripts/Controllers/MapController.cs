﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour {
    [Header("Home")]
    public Vector2 respawnPoint = Vector2.zero;

    [Header("Map Size")]
    public Vector2 bounds = Vector2.one;
    
    private ExplorationRoom[] explorationRooms;
    private CombatRoom[] combatRooms;
    private PassiveEnemy[] passiveEnemies;

    private List<Door> doors;

    private void Awake() {
        doors = new List<Door>();

        foreach (Door door in GetComponentsInChildren<Door>()) {
            doors.Add(door);
        }

        explorationRooms = GetComponentsInChildren<ExplorationRoom>();
        combatRooms = GetComponentsInChildren<CombatRoom>();
        passiveEnemies = GetComponentsInChildren<PassiveEnemy>();
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


    public void FadeInMap() {
        foreach(Room r in explorationRooms) {
            r.FadeIn();
        }
        foreach (PassiveEnemy pe in passiveEnemies) {
            pe.FadeIn();
        }
    }

    public void FadeOutMap() {
        foreach (Room r in explorationRooms) {
            r.FadeOut();
        }
        foreach (PassiveEnemy pe in passiveEnemies) {
            pe.FadeOut();
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(transform.position, bounds);

        Gizmos.DrawIcon(respawnPoint, "home.png", true);
        Gizmos.DrawWireSphere(respawnPoint, 0.05f);
    }
}
