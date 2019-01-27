using Anima2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PassiveEnemy : MonoBehaviour {
    public Transform lightSpriteTransform;
    public Transform darkSpriteTransform;

    public Vector2[] pathPoints;
    public float movingSpeed = 1;
    public float delayBetweenPoints = 2;

    private SpriteMeshInstance[] lightSpriteRenderers;
    private SpriteMeshInstance[] darkSpriteRenderers;

    private Collider2D[] colliders;
    private Animator[] animators;

    private ExplorationRoom assignedRoom;

    private bool completed;
    private bool fadingIn;
    private bool fadingOut;
    private float fadingSpeed;
    private float alpha;

    private bool moving;
    private float beta;
    private Vector2 destinationPoint;
    private Vector2 originPoint;
    private int pathIndex;

    private void Awake() {
        completed = false;

        lightSpriteRenderers = lightSpriteTransform.GetComponentsInChildren<SpriteMeshInstance>();
        darkSpriteRenderers = darkSpriteTransform.GetComponentsInChildren<SpriteMeshInstance>();

        foreach (SpriteMeshInstance sr in lightSpriteRenderers) {
            sr.color = new Color(1, 1, 1, 0);
        }

        fadingIn = false;
        fadingOut = false;
        moving = false;

        alpha = 1;
        beta = 0;
        pathIndex = 0;

        colliders = GetComponentsInChildren<Collider2D>();
        animators = GetComponentsInChildren<Animator>();

        Move();
    }

    public void AssignRoom(ExplorationRoom room) {
        assignedRoom = room;
        fadingSpeed = room.fadeSpeed;
    }
    
    public void Complete() {
        completed = true;
    }

    private void Update() {
        if (fadingIn) {
            alpha = Mathf.Clamp(alpha + fadingSpeed * Time.deltaTime, 0, 1);

            if (completed) {
                foreach (SpriteMeshInstance sr in lightSpriteRenderers) {
                    sr.color = new Color(1, 1, 1, alpha);
                }
            } else {
                foreach (SpriteMeshInstance sr in darkSpriteRenderers) {
                    sr.color = new Color(1, 1, 1, alpha);
                }
            }

            if (alpha == 1) {
                fadingIn = false;
                foreach (Collider2D c2d in colliders) {
                    c2d.enabled = true;
                }
            }
        } else if (fadingOut) {
            alpha = Mathf.Clamp(alpha - fadingSpeed * Time.deltaTime, 0, 1);

            if (completed) {
                foreach (SpriteMeshInstance sr in lightSpriteRenderers) {
                    sr.color = new Color(1, 1, 1, alpha);
                }
            } else {
                foreach (SpriteMeshInstance sr in darkSpriteRenderers) {
                    sr.color = new Color(1, 1, 1, alpha);
                }
            }

            if (alpha == 0) {
                fadingOut = false;
            }
        }

        if (moving) {
            beta += movingSpeed * Time.deltaTime;
            transform.position = Vector3.Lerp(new Vector3(originPoint.x, originPoint.y, 0), new Vector3(destinationPoint.x, destinationPoint.y, 0), beta);

            if (beta >= 1) {
                beta = 0;
                moving = false;
                foreach (Animator anim in animators) {
                    anim.SetBool("walk", false);
                }
                StartCoroutine(WaitAndMove());
            }
        }
    }

    public void FadeIn() {
        fadingOut = false;
        fadingIn = true;
    }

    public void FadeOut() {
        fadingOut = true;
        fadingIn = false;

        foreach (Collider2D c2d in colliders) {
            c2d.enabled = false;
        }
    }

    private void Move() {
        beta = 0;
        moving = true;
        foreach (Animator anim in animators) {
            anim.SetBool("walk", true);
        }
        originPoint = pathPoints[pathIndex];
        ++pathIndex;
        if (pathIndex == pathPoints.Length) {
            pathIndex = 0;
        }
        destinationPoint = pathPoints[pathIndex];

        if (originPoint.x < destinationPoint.x) {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        } else {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }

    private IEnumerator WaitAndMove() {
        yield return new WaitForSeconds(delayBetweenPoints);
        Move();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (!completed) {
            assignedRoom.StartRoom();
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.white;
        
        for (int i = 0; i < pathPoints.Length; ++i) {
            Gizmos.DrawWireSphere(new Vector3(pathPoints[i].x, pathPoints[i].y, 0), .5f);
            if (i < pathPoints.Length - 1) {
                Gizmos.DrawLine(new Vector3(pathPoints[i].x, pathPoints[i].y, 0), new Vector3(pathPoints[i + 1].x, pathPoints[i + 1].y, 0));
            } else {
                Gizmos.DrawLine(new Vector3(pathPoints[i].x, pathPoints[i].y, 0), new Vector3(pathPoints[0].x, pathPoints[0].y, 0));
            }
        }
    }
}
