using System;
using UnityEngine;

public class InputManager : MonoBehaviour {
    [SerializeField] private Camera sceneCamera;
    [SerializeField] LayerMask placementLayermask;

    private Vector3 lastPosition;

    public event Action OnClicked, OnExit;

    private void Update() {
        // Comprobar si hay al menos un toque en la pantalla
        if (Input.touchCount > 0) {
            Touch touch = Input.GetTouch(0); // Obtener el primer toque

            if (touch.phase == TouchPhase.Began) {
                OnClicked?.Invoke();
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
            OnExit?.Invoke();
    }

    public Vector3 GetSelectedMapPosition() {
        if (Input.touchCount > 0) {
            Vector3 touchPos = Input.GetTouch(0).position; // Obtener la posición del primer toque
            touchPos.z = sceneCamera.nearClipPlane; // Asegurarse de que se usa una distancia para la cámara
            Ray ray = sceneCamera.ScreenPointToRay(touchPos); // Convertir la posición del toque a un rayo
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100, placementLayermask)) {
                lastPosition = hit.point;
            }
        }
        return lastPosition;
    }
}
