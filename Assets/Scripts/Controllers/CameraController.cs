using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    [Header("Camera Adjust Options")]
    public float mapTransitionSpeed = 1f;
    public AnimationCurve cameraZoomCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
    public AnimationCurve cameraMovementCurve = AnimationCurve.Linear(0f,0f,1f,1f);

    private float transitionSpeed;

    private Camera mainCamera;
    private MapController mapController;

    private Vector3 cameraInitialPosition;
    private Vector3 cameraDestPosition;
    private float lerpMovementStep;
    private Coroutine movementCoroutine;

    private float cameraInitialSize;
    private float cameraDestSize;
    private float lerpZoomStep;
    private Coroutine zoomCoroutine;

    private float lastAspectRatio;

    private void Awake() {
        mainCamera = GetComponent<Camera>();
        mapController = GameObject.FindGameObjectWithTag("MapController").GetComponent<MapController>();
        lastAspectRatio = 0f;
    }

    private void Start() {
        //Comienzo del GamePlay con vista general del mapa
        FocusMap(true);
    }

    public float CalculateCameraSize(Vector2 bounds) {
        return Mathf.Max(bounds.y, bounds.x / mainCamera.aspect) / 2f;
    }

    public void FocusMap(bool inmediate = false) {
        if (inmediate) {
            transform.position = new Vector3(mapController.transform.position.x, mapController.transform.position.y, transform.position.z);
            mainCamera.orthographicSize = CalculateCameraSize(mapController.bounds);
        }
        else {
            FocusMap(mapTransitionSpeed);
        }
    }

    public void FocusMap(float fadeSpeed) {
        transitionSpeed = fadeSpeed;
        cameraDestPosition = new Vector3(mapController.transform.position.x, mapController.transform.position.y, transform.position.z);
        if (movementCoroutine != null) {
            StopCoroutine(movementCoroutine);
        }
        movementCoroutine = StartCoroutine(MoveCamera());

        cameraDestSize = CalculateCameraSize(mapController.bounds);
        if (zoomCoroutine != null) {
            StopCoroutine(zoomCoroutine);
        }
        zoomCoroutine = StartCoroutine(ZoomCamera());
    }


    public void FocusRoom(CombatRoom room, float fadingSpeed, bool inmediate = false) {
        if (inmediate) {
            transform.position = new Vector3(room.transform.position.x, room.transform.position.y, transform.position.z);
            mainCamera.orthographicSize = CalculateCameraSize(room.bounds);
        } else {
            transitionSpeed = fadingSpeed;
            cameraDestPosition = new Vector3(room.transform.position.x, room.transform.position.y, transform.position.z);
            if (movementCoroutine != null) {
                StopCoroutine(movementCoroutine);
            }
            movementCoroutine = StartCoroutine(MoveCamera());

            cameraDestSize = CalculateCameraSize(room.bounds);
            if (zoomCoroutine != null) {
                StopCoroutine(zoomCoroutine);
            }
            zoomCoroutine = StartCoroutine(ZoomCamera());
        }
    }

    private IEnumerator MoveCamera() {
        cameraInitialPosition = transform.position;
        lerpMovementStep = 0f;
        while (lerpMovementStep <= 1f) {
            lerpMovementStep += Time.deltaTime * transitionSpeed;
            transform.position = Vector3.Lerp(cameraInitialPosition, cameraDestPosition, cameraMovementCurve.Evaluate(lerpMovementStep));
            yield return null;
        }
        transform.position = cameraDestPosition;
    }

    private IEnumerator ZoomCamera() {
        cameraInitialSize = mainCamera.orthographicSize;
        lerpZoomStep = 0f;
        while (lerpZoomStep <= 1f) {
            lerpZoomStep += Time.deltaTime * transitionSpeed;
            mainCamera.orthographicSize = Mathf.Lerp(cameraInitialSize, cameraDestSize, cameraZoomCurve.Evaluate(lerpZoomStep));
            yield return null;
        }
        mainCamera.orthographicSize = cameraDestSize;
    }

    //Puede no interesar
    private void Update() {
        if (lastAspectRatio != mainCamera.aspect) {
            lastAspectRatio = mainCamera.aspect;
            FocusMap();
        }   
    }
}
