using System;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField] private GameObject mouseIndicator;
    [SerializeField] private GameObject cellIndicator;
    [SerializeField] private InputManager inputManager;
    [SerializeField] private Grid grid;

    [SerializeField] private TorretsData torrentsData;
    private int selectedTorrentIndex=-1;
    [SerializeField] private GameObject gridVisualization;


    private void Start() {
        StopPlacement();
    }
    public void StartPlacement(int ID) {
        StopPlacement();
        selectedTorrentIndex=torrentsData.torrentData.FindIndex(data =>data.ID==ID);
        gridVisualization.SetActive(true);
        cellIndicator.SetActive(true);
        inputManager.OnClicked += PlaceTorrent;
        inputManager.OnExit += StopPlacement;
    }
    private void PlaceTorrent() {
        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);
        GameObject newObject = Instantiate(torrentsData.torrentData[selectedTorrentIndex].Prefab);
        newObject.transform.position = grid.CellToWorld(gridPosition);
    }
    private void StopPlacement() {
        selectedTorrentIndex = -1;
        gridVisualization.SetActive(false);
        cellIndicator.SetActive(false);
        inputManager.OnClicked -= PlaceTorrent;
        inputManager.OnExit -= StopPlacement;
    }

    private void Update() {

        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);
        mouseIndicator.transform.position = mousePosition;
        cellIndicator.transform.position=grid.CellToWorld(gridPosition);

        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            StartPlacement(1);
        }
    }
}
