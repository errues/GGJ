using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour {

    public float speed = 6;

    private bool drivenMovemen = false;
    private Rigidbody2D r2D;

    private void Awake() {
        r2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate() {
        if (!drivenMovemen) {
            // Comprobamos si estamos intentando desplazarnos
            float horizontalMovement = Input.GetAxisRaw("Horizontal");
            if (horizontalMovement != 0) {
                transform.Translate(Vector3.right * speed * Time.fixedDeltaTime * horizontalMovement);
            }

            float verticalMovement = Input.GetAxisRaw("Vertical");
            if (verticalMovement != 0) {
                transform.Translate(Vector3.up * speed * Time.fixedDeltaTime * verticalMovement);
            }

            r2D.MovePosition(transform.position);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (LayerMask.LayerToName(collision.collider.gameObject.layer) == "Wall") {
            // reproducir sonido
        }
    }
}
