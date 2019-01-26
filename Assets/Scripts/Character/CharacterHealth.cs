using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHealth : MonoBehaviour {
    public RenderTexture attackTargetTexture;
    private Texture2D tex;
    private Rect rectReadPicture;

    private void Awake() {
        tex = new Texture2D(attackTargetTexture.width, attackTargetTexture.height);
        rectReadPicture = new Rect(0, 0, attackTargetTexture.width, attackTargetTexture.height);
    }

    private void Update() {
        RenderTexture.active = attackTargetTexture;

        tex.ReadPixels(rectReadPicture, 0, 0);
        tex.Apply();

        if (AttackDetected()) {
            //Die
            print("MUERRRRRRRTO");
        }

        RenderTexture.active = null;
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
}
