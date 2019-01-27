using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatRoom : Room {
    public Vector2 bounds = Vector2.one;
    public Vector2 initialPoint;

    public AudioClip ostClip;

    public bool printTime = false;

    public AttackParameters[] attacks;

    private SpriteRenderer spriteRenderer;
    private Collider2D[] colliders;

    private ExplorationRoom assignedRoom;
    private CanvasController canvasController;

    private float roomTime;
    private int attackIndex;

    private Coroutine waitingToFinish;
    private Coroutine nextAttack;

    protected override void Awake() {
        base.Awake();

        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        colliders = GetComponentsInChildren<Collider2D>();

        canvasController = GameObject.FindGameObjectWithTag("Canvas").GetComponent<CanvasController>();
    }

    private void Start() {
        // Calculamos el tiempo de la habitación, en función de sus ataques
        roomTime = 0;
        int i = 0;
        List<float> simultaneousTimes = new List<float>();
        while (i < attacks.Length) {
            attacks[i].realTime = attacks[i].attack.GetLength() / attacks[i].speed + attacks[i].delay;
            attacks[i].originalRotation = attacks[i].attack.transform.rotation;
            simultaneousTimes.Add(attacks[i].realTime);

            while (i < attacks.Length - 1 && attacks[i + 1].simultaneoausWithPrevious) {
                attacks[i + 1].realTime = attacks[i + 1].attack.GetLength() / attacks[i + 1].speed + attacks[i + 1].delay;
                attacks[i + 1].originalRotation = attacks[i + 1].attack.transform.rotation;
                simultaneousTimes.Add(attacks[i + 1].realTime);
                ++i;
            }

            // Cogemos el más grande
            float maxTime = 0;
            foreach (float f in simultaneousTimes) {
                if (f > maxTime) {
                    maxTime = f;
                }
            }

            roomTime += maxTime;
            simultaneousTimes.Clear();

            ++i;
        }

        foreach (AttackParameters ap in attacks) {
            ap.attack.gameObject.SetActive(false);
        }

        DeactivateColliders();

        if (printTime) {
            print("time in " + name + ": " + roomTime);
        }
    }

    public void AssignRoom(ExplorationRoom room) {
        assignedRoom = room;
        fadingSpeed = room.fadeSpeed;
    }

    public void Hide() {
        alpha = 0;
        spriteRenderer.color = new Color(1, 1, 1, 0);
    }

    protected override void Update() {
        base.Update();

        if (fadingIn || fadingOut) {
            spriteRenderer.color = new Color(1, 1, 1, alpha);
        }

        CheckFadings();
    }

    public virtual void Run() {
        attackIndex = 0;
        canvasController.StartTimeBar(this);
        if (attacks.Length > 0) {
            nextAttack = StartCoroutine(NextAttack());
        }

        waitingToFinish = StartCoroutine(WaitAndFinish(roomTime));
    }

    private IEnumerator NextAttack() {
        // Guardamos en una lista todos los ataques simultáneos
        List<int> simultaneousAttacksIndexes = new List<int>();
        simultaneousAttacksIndexes.Add(attackIndex);

        while (attackIndex < attacks.Length - 1 && attacks[attackIndex + 1].simultaneoausWithPrevious) {
            simultaneousAttacksIndexes.Add(attackIndex + 1);
            ++attackIndex;
        }

        // Calculamos cuál es el ataque más largo
        float longestTime = 0;
        int longestAttack = -1;
        foreach (int i in simultaneousAttacksIndexes) {
            if (attacks[i].realTime > longestTime) {
                longestAttack = i;
            }
        }

        // Lanzamos una corrutina simplificada por cada uno de los ataques que no sean el más largo
        foreach (int i in simultaneousAttacksIndexes) {
            if (i != longestAttack) {
                StartCoroutine(DoAttack(i));
            }
        }

        // Realizamos el ataque más largo
        // Esperamos el delay
        yield return new WaitForSeconds(attacks[longestAttack].delay);

        // Ajustamos velocidad y rotación, y lanzamos
        attacks[longestAttack].attack.gameObject.SetActive(true);
        if (attacks[longestAttack].randomRotation) {
            attacks[longestAttack].attack.transform.Rotate(new Vector3(0, 0, 90 * Random.Range(0, 4)));
        } else {
            attacks[longestAttack].attack.transform.rotation = attacks[longestAttack].originalRotation;
        }
        attacks[longestAttack].attack.Run(attacks[longestAttack].speed);

        // Esperamos a que termine
        yield return new WaitForSeconds(attacks[longestAttack].attack.GetLength() / attacks[longestAttack].speed);
        // Desactivamos
        attacks[longestAttack].attack.gameObject.SetActive(false);

        // Pasamos al siguiente
        ++attackIndex;
        if (attackIndex < attacks.Length) {
            nextAttack = StartCoroutine(NextAttack());
        } else {
            // termina la habitación
        }
    }

    private IEnumerator DoAttack(int index) {
        // Esperamos el delay
        yield return new WaitForSeconds(attacks[index].delay);

        // Ajustamos velocidad y rotación, y lanzamos
        attacks[index].attack.gameObject.SetActive(true);
        if (attacks[index].randomRotation) {
            attacks[index].attack.transform.Rotate(new Vector3(0, 0, 90 * Random.Range(0, 4)));
        } else {
            attacks[index].attack.transform.rotation = attacks[index].originalRotation;
        }
        attacks[index].attack.Run(attacks[index].speed);

        // Esperamos a que termine
        yield return new WaitForSeconds(attacks[index].attack.GetLength() / attacks[index].speed);
        // Desactivamos
        attacks[index].attack.gameObject.SetActive(false);
    }

    private IEnumerator WaitAndFinish(float time) {
        yield return new WaitForSeconds(time);
        
        assignedRoom.FinishRoom();
    }

    public void CharacterDied() {
        if (waitingToFinish != null) {
            StopCoroutine(waitingToFinish);
        }
    }

    public float GetRoomTime() {
        return roomTime;
    }

    public void StopAllAttacks() {
        StopAllCoroutines();

        foreach (AttackParameters ap in attacks) {
            ap.attack.gameObject.SetActive(false);
        }
    }

    public void ActivateColliders() {
        foreach (Collider2D col in colliders) {
            col.enabled = true;
        }
    }

    public void DeactivateColliders() {
        foreach (Collider2D col in colliders) {
            col.enabled = false;
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(transform.position, bounds);

        Gizmos.color = Color.green;
        Gizmos.DrawSphere(initialPoint, (transform.localScale.x + transform.localScale.y) / 2);
    }
}
