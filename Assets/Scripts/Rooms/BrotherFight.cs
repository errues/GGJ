using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrotherFight : CombatRoom {
    public ExplorationRoom grandmotherRoom;
    public ExplorationRoom motherRoom;
    public ExplorationRoom fatherRoom;
    public ExplorationRoom dogRoom;

    public GameObject grandmotherSafePoint;
    public GameObject motherSafePoint;
    public GameObject fatherSafePoint;
    public GameObject dogSafePoint;

    private bool grandmotherErased;
    private bool motherErased;
    private bool fatherErased;
    private bool dogErased;

    private Character character;

    protected override void Awake() {
        base.Awake();

        grandmotherSafePoint.SetActive(false);
        motherSafePoint.SetActive(false);
        fatherSafePoint.SetActive(false);
        dogSafePoint.SetActive(false);

        character = GameObject.FindGameObjectWithTag("Character").GetComponent<Character>();
    }

    public override void Run() {
        base.Run();

        grandmotherErased = !grandmotherRoom.Enlighted;
        motherErased = !motherRoom.Enlighted;
        fatherErased = !fatherRoom.Enlighted;
        dogErased = !dogRoom.Enlighted;

        grandmotherSafePoint.SetActive(false);
        motherSafePoint.SetActive(false);
        fatherSafePoint.SetActive(false);
        dogSafePoint.SetActive(false);
    }

    protected override IEnumerator WaitAndFinish(float time) {
        yield return new WaitForSeconds(time);

        GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().ActivateLastFight();
        assignedRoom.FinishRoom();
    }

    public void StartBrotherAttack() {
        if (grandmotherRoom.Enlighted && !grandmotherErased) {
            grandmotherSafePoint.SetActive(true);
        } else {
            grandmotherSafePoint.SetActive(false);
        }

        if (fatherRoom.Enlighted && !fatherErased) {
            fatherSafePoint.SetActive(true);
        } else {
            fatherSafePoint.SetActive(false);
        }

        if (motherRoom.Enlighted && !motherErased) {
            motherSafePoint.SetActive(true);
        } else {
            motherSafePoint.SetActive(false);
        }

        if (dogRoom.Enlighted && !dogErased) {
            dogSafePoint.SetActive(true);
        } else {
            dogSafePoint.SetActive(false);
        }
    }

    public void EndBrotherAttack() {
        grandmotherSafePoint.SetActive(false);
        motherSafePoint.SetActive(false);
        fatherSafePoint.SetActive(false);
        dogSafePoint.SetActive(false);

        if (character.CharacterHealth.IsAlive) {
            float minDistance = float.MaxValue;
            GameObject selected = null;

            if (!grandmotherErased && Vector3.Distance(grandmotherSafePoint.transform.position, character.transform.position) < minDistance) {
                selected = grandmotherSafePoint;
                minDistance = Vector3.Distance(grandmotherSafePoint.transform.position, character.transform.position);
            }
            if (!motherErased && Vector3.Distance(motherSafePoint.transform.position, character.transform.position) < minDistance) {
                selected = motherSafePoint;
                minDistance = Vector3.Distance(motherSafePoint.transform.position, character.transform.position);
            }
            if (!fatherErased && Vector3.Distance(fatherSafePoint.transform.position, character.transform.position) < minDistance) {
                selected = fatherSafePoint;
                minDistance = Vector3.Distance(fatherSafePoint.transform.position, character.transform.position);
            }
            if (!dogErased && Vector3.Distance(dogSafePoint.transform.position, character.transform.position) < minDistance) {
                selected = dogSafePoint;
                minDistance = Vector3.Distance(dogSafePoint.transform.position, character.transform.position);
            }

            if (selected == grandmotherSafePoint) {
                grandmotherErased = true;
            } else if (selected == motherSafePoint) {
                motherErased = true;
            } else if (selected == fatherSafePoint) {
                fatherErased = true;
            } else if (selected == dogSafePoint) {
                dogErased = true;
            }
        }
    }

    /*
      protected override void Awake() {
        base.Awake();

        grandmotherSafePoint.SetActive(false);
        motherSafePoint.SetActive(false);
        fatherSafePoint.SetActive(false);
        dogSafePoint.SetActive(false);

        grandmotherErased = grandmotherRoom.Enlighted;
        motherErased = motherRoom.Enlighted;
        fatherErased = fatherRoom.Enlighted;
        dogErased = dogRoom.Enlighted;

        character = GameObject.FindGameObjectWithTag("Character").GetComponent<Character>();
    }    

    public void StartBrotherAttack() {
        if (grandmotherRoom.Enlighted && !grandmotherErased) {
            grandmotherSafePoint.SetActive(true);
        } else {
            grandmotherSafePoint.SetActive(false);
        }

        if (fatherRoom.Enlighted && !motherErased) {
            fatherSafePoint.SetActive(true);
        } else {
            fatherSafePoint.SetActive(false);
        }

        if (motherRoom.Enlighted && !fatherErased) {
            motherSafePoint.SetActive(true);
        } else {
            motherSafePoint.SetActive(false);
        }

        if (dogRoom.Enlighted && !dogErased) {
            dogSafePoint.SetActive(true);
        } else {
            dogSafePoint.SetActive(false);
        }
    }
     */
}
