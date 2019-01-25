using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHealth : MonoBehaviour {
    public RenderTexture attackTargetTexture;
    private Texture2D tex;
    private Rect rectReadPicture;

    private void Awake() {
        tex = new Texture2D(128, 128);
        rectReadPicture = new Rect(0, 0, 128, 128);
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
        for (int x = 1; x <= 128; x += 32) {
            for (int y = 1; y <= 128; y += 32) {
                if (tex.GetPixel(x - 1, y - 1).a != 0) {
                    return true;
                }
            }
        }
        return false;
    }
}
