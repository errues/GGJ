using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {
    public bool iniciateGame = true;

    public Character player;
    public ExplorationRoom characterRoom;
    public Door characterDoor;
    public CombatRoom lastFight;
    public PassiveEnemy finalEnemy;

    public List<Vector2> initialCharacterPath = new List<Vector2>();

    private MapController mapController;
    private OSTController ostController;
    private CanvasController canvasController;

    private void Awake() {
        mapController = GameObject.FindGameObjectWithTag("MapController").GetComponent<MapController>();
        ostController = mapController.GetComponent<OSTController>();
        canvasController = GameObject.FindGameObjectWithTag("Canvas").GetComponent<CanvasController>();
    }

    private void Start() {
        if (iniciateGame) {
            StartCoroutine(StartGame());
        }

        //Comienza la música oscura
        ostController.PlayDarkTheme();
    }

    private IEnumerator StartGame() {
        //Establece el path inicial del character y espera a que lo complete
        player.CharacterMovement.DrivenMovement(initialCharacterPath);
        yield return new WaitWhile(() => player.CharacterMovement.IsInDrivenMovement);

        //Desactiva la interacción con las puertas
        mapController.DissableDoors();

        do {
            //Pequeña espera de tiempo
            yield return new WaitForSeconds(2f);

            //Inicia el combate de la habitación inicial
            characterRoom.StartRoom();

            //Reinicia si muere el personaje
            yield return new WaitUntil(() => !player.CharacterHealth.IsAlive || characterRoom.Enlighted);
            if (!player.CharacterHealth.IsAlive) {
                yield return new WaitUntil(() => player.CharacterHealth.IsAlive);
            }
        } while (!characterRoom.Enlighted);

        //Reactiva las puertas del mapa
        mapController.EnableDoors();
    }

    public void ActivateLastFight() {
        mapController.EnlightAll();

        //Asigna la nueva CombatRoom
        characterRoom.Enlighted = false;
        characterRoom.AssignCombatRoom(lastFight);

        //Eliminamos todos los pasive enemies
        /*for (int i = mapController.enemiesTransform.childCount - 1; i >= 0; i--) {
            Destroy(mapController.enemiesTransform.GetChild(i));
        }*/

        //Asigna el nuevo pasive enemy
        finalEnemy.transform.SetParent(mapController.enemiesTransform);
        finalEnemy.gameObject.SetActive(true);
    }

    public void FinishGame() {
        Destroy(finalEnemy);
        StartCoroutine(LaunchCredits());
    }

    private IEnumerator LaunchCredits() {
        yield return new WaitForSeconds(4f);

        //Espera al fade en negro
        canvasController.FadeIn();
        yield return new WaitUntil(() => !canvasController.IsFadingIn);

        //Carga asíncrona de los créditos
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Creditos");
        while (!asyncLoad.isDone) {
            yield return null;
        }
    }
}
