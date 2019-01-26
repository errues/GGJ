using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplorationRoom : Room {
    public SpriteRenderer lightRoomSprite;
    public SpriteRenderer darkRoomSprite;
    public float fadeSpeed = 1;

    public PassiveEnemy assignedEnemy;
    public CombatRoom assignedCombatRoom;

    private CharacterMovement characterMovement;
    private MapController mapController;
    private CameraController cameraController;

    private bool enlighted;

    protected override void Awake() {
        base.Awake();
        fadingSpeed = fadeSpeed;

        characterMovement = GameObject.FindGameObjectWithTag("Character").GetComponent<CharacterMovement>();
        mapController = GameObject.FindGameObjectWithTag("MapController").GetComponent<MapController>();
        cameraController = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>();

        lightRoomSprite.color = new Color(1, 1, 1, 0);
    }

    private void Start() {
        assignedEnemy.AssignRoom(this);
        assignedCombatRoom.AssignRoom(this);
    }

    public void AssignEnemy(PassiveEnemy enemy) {
        assignedEnemy = enemy;
    }

    public void AssignCombatRoom(CombatRoom room) {
        assignedCombatRoom = room;
    }

    public void StartRoom() {
        if (assignedCombatRoom != null) {
            // Bloqueamos interacción del personaje
            characterMovement.EnabledInteraction = false;

            // Lo mandamos a la posición de inicio de la habitación de combate
            List<Vector2> path = new List<Vector2>();
            path.Add(assignedCombatRoom.initialPoint);
            characterMovement.DrivenMovement(path);

            // Hacemos la transición de habitaciones
            mapController.FadeOutMap();
            assignedCombatRoom.FadeIn();
            assignedEnemy.FadeOut();
            cameraController.FocusRoom(assignedCombatRoom, fadeSpeed);

            StartCoroutine(WaitAndRun());
        }
    }

    private IEnumerator WaitAndRun() {
        yield return new WaitUntil(() => fadingOut == false);
        print(1);
        yield return new WaitUntil(() => assignedCombatRoom.IsFadingIn == false);
        print(2);
        yield return new WaitUntil(() => characterMovement.IsInDrivenMovement == false);
        print(3);

        characterMovement.EnabledInteraction = true;
        assignedCombatRoom.Run();
    }

    protected override void Update() {
        base.Update();

        if (fadingIn || fadingOut) {
            if (enlighted) {
                lightRoomSprite.color = new Color(1, 1, 1, alpha);
            } else {
                darkRoomSprite.color = new Color(1, 1, 1, alpha);
            }
        }

        CheckFadings();
    }

    public void Enlighten() {
        enlighted = true;
    }
}
