using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    [SerializeField]
    private Camera sceneCamera;

    private Vector3 lastPosition;

    [SerializeField]
    private LayerMask placementLayermask;

    public event Action OnClicked, OnExit;

    private void Update()
    {
        // Detectar toques en la pantalla en lugar de clicks del ratón
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0); // Obtener el primer toque

            if (touch.phase == TouchPhase.Began)
            {
                OnClicked?.Invoke();
            }
        }

        // Detectar la presión de la tecla Escape (solo se usaría en editor o con teclado en Android)
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnExit?.Invoke();
        }
    }

    public bool IsPointerOverUI()
    {
        // Verificar si el dedo o puntero está sobre un UI
        return EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId);
    }

    public Vector3 GetSelectedMapPosition()
    {
        // Obtener la posición del toque en la pantalla
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 touchPos = touch.position;
            touchPos.z = sceneCamera.nearClipPlane;

            Ray ray = sceneCamera.ScreenPointToRay(touchPos);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100, placementLayermask))
            {
                lastPosition = hit.point;
            }
        }
        return lastPosition;
    }
}
