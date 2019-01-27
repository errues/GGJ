using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHealth : MonoBehaviour {
    public RenderTexture attackTargetTexture;
    public GameObject deathAnimation;

    private Texture2D tex;
    private Rect rectReadPicture;

    private ExplorationRoom currentRoom;

    private bool alive;

    public bool IsAlive {
        get {
            return alive;
        }
    }

    private void Awake() {
        tex = new Texture2D(attackTargetTexture.width, attackTargetTexture.height);
        rectReadPicture = new Rect(0, 0, attackTargetTexture.width, attackTargetTexture.height);

        alive = true;
    }

    public void AssignRoom(ExplorationRoom room) {
        currentRoom = room;
    }

    private void Update() {
        if (alive) {
            RenderTexture.active = attackTargetTexture;

            tex.ReadPixels(rectReadPicture, 0, 0);
            tex.Apply();

            if (AttackDetected()) {
                //Die
                currentRoom.CharacterDied();
                SetAlive(false);
            }

            RenderTexture.active = null;
        }
    }

    private bool AttackDetected() {
        for (int x = 0; x < attackTargetTexture.width; ++x) {
            for (int y = 0; y < attackTargetTexture.height; ++y) {
                if (tex.GetPixel(x, y).a != 0) {
                    return true;
                }
            }
        }
        return false;
    }

    public void SetAlive(bool alive) {
        this.alive = alive;
    }

    public void PlayDeathAnimation() {
        deathAnimation.SetActive(false);
        deathAnimation.SetActive(true);
    }

    public void GoGood() {
        deathAnimation.GetComponent<DeathAnimation>().GoGood();
    }
}
