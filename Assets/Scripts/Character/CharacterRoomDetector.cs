using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterRoomDetector : MonoBehaviour {

    private ExplorationRoom enteredRoom;
    private OSTController ostController;

    public ExplorationRoom CurrentRoom {
        get {
            return enteredRoom;
        }
    }

    private void Awake() {
        ostController = GameObject.FindGameObjectWithTag("MapController").GetComponent<OSTController>();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        ExplorationRoom room = collision.gameObject.GetComponent<ExplorationRoom>();
        if (room != null) {
            //Si no tenia ya una habitación
            if (enteredRoom == null) {
                if (room.Enlighted) {
                    ostController.PlayLightTheme();
                }
                else {
                    ostController.PlayDarkTheme();
                }
            }

            enteredRoom = room;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        ExplorationRoom room = collision.gameObject.GetComponent<ExplorationRoom>();
        if (room != null) {
            if (room != enteredRoom) {
                if (room.Enlighted) {
                    ostController.PlayLightTheme();
                }
                else {
                    ostController.PlayDarkTheme();
                }
            } else {
                enteredRoom = null;
            }
        }
    }

    public void ClearRoom() {
        enteredRoom = null;
    }
}
