using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrotherAttack : Attack {
    public BrotherFight brotherFight;

    public override void Run(float speed) {
        base.Run(speed);
        brotherFight.StartBrotherAttack();

        StartCoroutine(WaitAndCall());
    }

    private IEnumerator WaitAndCall() {
        yield return new WaitForSeconds(4);
        brotherFight.EndBrotherAttack();
    }
}
