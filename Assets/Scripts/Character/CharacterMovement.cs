using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour {

    public float speed = 6;

    private void Update() {
        // Comprobamos si estamos intentando desplazarnos
        float horizontalMovement = Input.GetAxisRaw("Horizontal");
        if (horizontalMovement != 0) {
            transform.Translate(Vector3.right * speed * Time.deltaTime * horizontalMovement);
        }

        float verticalMovement = Input.GetAxisRaw("Vertical");
        if (verticalMovement != 0) {
            transform.Translate(Vector3.up * speed * Time.deltaTime * verticalMovement);
        }
    }
}
