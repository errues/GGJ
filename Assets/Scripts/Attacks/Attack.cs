using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour {
    private Animator animator;

    private void Awake() {
        animator = GetComponentInChildren<Animator>();
    }

    public void Run(float speed) {
        animator.SetFloat("speed", speed);
    }

    public float GetLength() {
        return animator.GetCurrentAnimatorStateInfo(0).length;
    }
}
