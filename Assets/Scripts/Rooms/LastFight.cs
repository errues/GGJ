using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastFight : CombatRoom {
    protected override IEnumerator WaitAndFinish(float time) {
        yield return new WaitForSeconds(time);

        GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().FinishGame();
    }
}
