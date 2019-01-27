using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour {

    [Header("Character Speed")]
    public float movementSpeed = 6;
    public float drivenMovementSpeed = 10;

    [Header("Character Driven Path")]
    [SerializeField]
    private List<Vector2> drivenPath = new List<Vector2>();

    [Header("Character Sprites")]
    [SerializeField]
    private Transform characterSprites;
    public bool isFacingRight = true;

    private Animator characterAnimator;

    private Vector3 initialMovementPosition;
    private Vector3 destMovementPosition;
    private float normalizedDrivenMovementSpeed;
    private float lerpMovementStep;

    private bool drivenMovemen = false;
    private Vector3 savedLocation;

    public bool EnabledInteraction { get; set; }
    public bool IsInDrivenMovement {
        get {
            return drivenMovemen;
        }
    }

    private void Awake() {
        characterAnimator = GetComponentInChildren<Animator>();

        if (drivenPath.Count > 0) {
            DoNextDrivenMovement();
        }
        if(characterSprites == null) {
            characterSprites = transform;
        }

        EnabledInteraction = true;
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

            //Coloca el personaje mirando hacia la dirección de movimiento
            if (destMovementPosition.x - initialMovementPosition.x > 0 && !isFacingRight) {
                isFacingRight = !isFacingRight;
                characterSprites.rotation = Quaternion.Euler(characterSprites.rotation.eulerAngles.x, 0f, characterSprites.rotation.eulerAngles.z);
            }
            else if (destMovementPosition.x - initialMovementPosition.x < 0 && isFacingRight) {
                isFacingRight = !isFacingRight;
                characterSprites.rotation = Quaternion.Euler(characterSprites.rotation.eulerAngles.x, 180f, characterSprites.rotation.eulerAngles.z);
            }

        } else {
            drivenMovemen = false;
        }

        //Establece la animación de anadar
        characterAnimator.SetBool("walk", drivenMovemen);
    }

    public void SaveCurrentLocation() {
        savedLocation = transform.position;
    }

    public void GoToSavedLocation() {
        drivenPath = new List<Vector2>();
        drivenPath.Add(savedLocation);
        DoNextDrivenMovement();
    }

    private void FixedUpdate() {
        if (!drivenMovemen && EnabledInteraction) {
            //Desplaza el personaje en el eje horizontal
            float horizontalMovement = Input.GetAxisRaw("Horizontal");
            if (horizontalMovement != 0) {
                transform.Translate(Vector3.right * movementSpeed * Time.fixedDeltaTime * horizontalMovement);
            }
            //Desplaza el personaje en el eje vertical
            float verticalMovement = Input.GetAxisRaw("Vertical");
            if (verticalMovement != 0) {
                transform.Translate(Vector3.up * movementSpeed * Time.fixedDeltaTime * verticalMovement);
            }

            //Coloca el personaje mirando hacia la dirección de movimiento
            if (horizontalMovement > 0 && !isFacingRight) {
                isFacingRight = !isFacingRight;
                characterSprites.rotation = Quaternion.Euler(characterSprites.rotation.eulerAngles.x, 0f, characterSprites.rotation.eulerAngles.z);
            } else if (horizontalMovement < 0 && isFacingRight) {
                isFacingRight = !isFacingRight;
                characterSprites.rotation = Quaternion.Euler(characterSprites.rotation.eulerAngles.x, 180f, characterSprites.rotation.eulerAngles.z);
            }

            //Establece la animación de andar
            characterAnimator.SetBool("walk", (horizontalMovement != 0 || verticalMovement != 0));
        } else if (drivenMovemen) {
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

        if (LayerMask.LayerToName(collision.collider.gameObject.layer) == "Door") {
            collision.collider.GetComponentInChildren<Door>().PlayClosedDoorHit();
        }
    }
}
