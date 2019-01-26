using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour {

    [Header("Character Speed")]
    public float movementSpeed = 6;
    public float drivenMovementSpeed = 10;

    [Header("Character Driven Path")]
    public List<Vector2> drivenPath = new List<Vector2>();

    private bool drivenMovemen = false;
    private Rigidbody2D r2D;

    private Vector3 initialMovementPosition;
    private Vector3 destMovementPosition;
    private float normalizedDrivenMovementSpeed;
    private float lerpMovementStep;

    public bool EnabledInteraction { get; set; }

    private void Awake() {
        r2D = GetComponent<Rigidbody2D>();
        if(drivenPath.Count > 0) {
            DoNextDrivenMovement();
        }
    }

    public void DrivenMovement(List<Vector2> drivenPath) {
        this.drivenPath = drivenPath;
        DoNextDrivenMovement();
    }

    private void DoNextDrivenMovement() {
        if (drivenPath.Count > 0) {
            drivenMovemen = true;
            initialMovementPosition = transform.position;
            destMovementPosition = new Vector3(drivenPath[0].x, drivenPath[0].y, transform.position.z);
            normalizedDrivenMovementSpeed = drivenMovementSpeed / Vector3.Distance(initialMovementPosition, destMovementPosition);
            lerpMovementStep = 0f;
            drivenPath.RemoveAt(0);
        } else {
            drivenMovemen = false;
        }
    }

    private void FixedUpdate() {
        if (!drivenMovemen && EnabledInteraction) {
            // Comprobamos si estamos intentando desplazarnos
            float horizontalMovement = Input.GetAxisRaw("Horizontal");
            if (horizontalMovement != 0) {
                transform.Translate(Vector3.right * movementSpeed * Time.fixedDeltaTime * horizontalMovement);
            }

            float verticalMovement = Input.GetAxisRaw("Vertical");
            if (verticalMovement != 0) {
                transform.Translate(Vector3.up * movementSpeed * Time.fixedDeltaTime * verticalMovement);
            }

            r2D.MovePosition(transform.position);
        } else {
            if (lerpMovementStep <= 1f) {
                lerpMovementStep += normalizedDrivenMovementSpeed * Time.fixedDeltaTime;
                transform.position = Vector3.Lerp(initialMovementPosition, destMovementPosition, lerpMovementStep);
            } else {
                transform.position = destMovementPosition;
                DoNextDrivenMovement();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (LayerMask.LayerToName(collision.collider.gameObject.layer) == "Wall") {
            // reproducir sonido
        }
    }
}
