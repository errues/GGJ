using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {
    private CharacterMovement characterMovement;
    private CharacterHealth characterHealth;
    private CharacterGraphics characterGraphics;
    private CharacterInteractor characterInteractor;
    private CharacterRoomDetector characterRoomDetector;

    public CharacterMovement CharacterMovement {
        get {
            return characterMovement;
        }
    }

    public CharacterHealth CharacterHealth {
        get {
            return characterHealth;
        }
    }

    public CharacterGraphics CharacterGraphics {
        get {
            return characterGraphics;
        }
    }

    public CharacterInteractor CharacterInteractor {
        get {
            return characterInteractor;
        }
    }

    public CharacterRoomDetector CharacterRoomDetector {
        get {
            return characterRoomDetector;
        }
    }

    private void Awake() {
        characterMovement = GetComponent<CharacterMovement>();
        characterHealth = GetComponent<CharacterHealth>();
        characterGraphics = GetComponent<CharacterGraphics>();
        characterInteractor = GetComponent<CharacterInteractor>();
        characterRoomDetector = GetComponent<CharacterRoomDetector>();
    }
}
