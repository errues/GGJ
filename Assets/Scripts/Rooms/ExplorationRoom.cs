using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplorationRoom : Room {
    public SpriteRenderer lightRoomSprite;
    public SpriteRenderer darkRoomSprite;
    public float fadeSpeed = 1;

    public PassiveEnemy assignedEnemy;
    public CombatRoom assignedCombatRoom;

    private Character character;

    private MapController mapController;
    private CameraController cameraController;
    private CanvasController canvasController;

    private bool enlighted;

    public bool Enlighted {
        get {
            return enlighted;
        }
    }

    protected override void Awake() {
        base.Awake();
        fadingSpeed = fadeSpeed;

        character = GameObject.FindGameObjectWithTag("Character").GetComponent<Character>();
        mapController = GameObject.FindGameObjectWithTag("MapController").GetComponent<MapController>();
        cameraController = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>();
        canvasController = GameObject.FindGameObjectWithTag("Canvas").GetComponent<CanvasController>();

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
            // Bloqueamos interacción del personaje y le asignamos la habitación de combate
            character.CharacterMovement.EnabledInteraction = false;
            character.CharacterHealth.AssignRoom(this);

            // Lo mandamos a la posición de inicio de la habitación de combate, después de guardar su posición actual
            character.CharacterMovement.SaveCurrentLocation();
            List<Vector2> path = new List<Vector2>();
            path.Add(assignedCombatRoom.initialPoint);
            character.CharacterMovement.DrivenMovement(path);

            // Hacemos la transición de habitaciones
            mapController.FadeOutMap();
            DeactivateColliders();
            assignedCombatRoom.FadeIn();
            assignedEnemy.FadeOut();
            cameraController.FocusRoom(assignedCombatRoom, fadeSpeed);

            StartCoroutine(WaitAndRun());
        }
    }

    public void FinishRoom() {
        if (character.CharacterHealth.IsAlive) {
            // Bloqueamos interacción del personaje y le quitamos la habitación de combate
            character.CharacterMovement.EnabledInteraction = false;
            character.CharacterHealth.AssignRoom(null);
        
            // Mandamos al personaje a su posición anterior
            character.CharacterMovement.GoToSavedLocation();

            // Marcamos la habitación y el enemigo como completados
            enlighted = true;
            assignedEnemy.Complete();
        
            // Hacemos la transición entre habitaciones
            mapController.FadeInMap();
            assignedCombatRoom.DeactivateColliders();
            assignedCombatRoom.FadeOut();
            assignedEnemy.FadeIn();
            cameraController.FocusMap(fadingSpeed);

            StartCoroutine(WaitAndContinueGame());
        }
    }

    public void CharacterDied() {
        // Bloqueamos interacción del personaje y le quitamos la habitación de combate
        character.CharacterMovement.EnabledInteraction = false;
        character.CharacterHealth.AssignRoom(null);
        character.CharacterHealth.SetAlive(false);

        // Paramos la corrutina de esperar a que termine
        assignedCombatRoom.CharacterDied();

        // Lanzamos la animación de muerte
        StartCoroutine(CharacterDeath());
    }

    private IEnumerator CharacterDeath() {
        // Esperamos un segundo
        yield return new WaitForSeconds(1);

        // Hacemos un fade del canvas, personaje y habitación
        canvasController.FadeIn();
        character.CharacterGraphics.FadeOut(fadingSpeed);
        assignedCombatRoom.FadeOut();

        // Esperamos a que todo esté apagado y un segundo más
        yield return new WaitUntil(() => fadingOut == false);
        yield return new WaitUntil(() => character.CharacterGraphics.IsFadingOut == false);
        yield return new WaitUntil(() => canvasController.IsFadingOut == false);
        yield return new WaitForSeconds(1);

        // Posicionamos al personaje en casa
        character.transform.position = new Vector3(mapController.respawnPoint.x, mapController.respawnPoint.y, character.transform.position.z);

        // Reseteamos el timebar
        canvasController.TerminateTimeBar();

        // Detenemos los ataques
        assignedCombatRoom.StopAllAttacks();

        // Hacemos un fade del personaje, de las habitaciones y del canvas
        character.CharacterGraphics.FadeIn(fadingSpeed);
        mapController.FadeInMap();
        canvasController.FadeOut();

        // Esperamos a que terminen los fades
        yield return new WaitUntil(() => fadingIn == false);
        yield return new WaitUntil(() => character.CharacterGraphics.IsFadingIn == false);

        // Habilitamos el control del personaje y su salud
        character.CharacterMovement.EnabledInteraction = true;
        character.CharacterHealth.SetAlive(true);
    }

    private IEnumerator WaitAndRun() {
        yield return new WaitUntil(() => fadingOut == false);
        yield return new WaitUntil(() => assignedCombatRoom.IsFadingIn == false);
        yield return new WaitUntil(() => character.CharacterMovement.IsInDrivenMovement == false);

        assignedCombatRoom.ActivateColliders();
        character.CharacterMovement.EnabledInteraction = true;
        assignedCombatRoom.Run();
    }

    private IEnumerator WaitAndContinueGame() {
        yield return new WaitUntil(() => fadingIn == false);
        yield return new WaitUntil(() => assignedCombatRoom.IsFadingOut == false);
        yield return new WaitUntil(() => character.CharacterMovement.IsInDrivenMovement == false);

        ActivateColliders();
        character.CharacterMovement.EnabledInteraction = true;
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
}
