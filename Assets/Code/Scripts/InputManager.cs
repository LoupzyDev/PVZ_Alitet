using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private Camera sceneCamera;
    [SerializeField] LayerMask placementLayermask;

    private Vector3 lastPosition;

    public event Action OnClicked, OnExit;

    private void Update() {
        if (Input.GetMouseButtonDown(0)) 
            OnClicked?.Invoke();
        if (Input.GetKeyDown(KeyCode.Escape))
            OnExit?.Invoke();
    }
    public Vector3 GetSelectedMapPosition() {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = sceneCamera.nearClipPlane;
        Ray ray = sceneCamera.ScreenPointToRay(mousePos);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit,100, placementLayermask)) {
            lastPosition = hit.point;
        }
        return lastPosition;
    }
}
