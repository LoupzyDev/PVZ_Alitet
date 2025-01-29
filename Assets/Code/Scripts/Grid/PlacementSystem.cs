using System;
using System.Collections.Generic;
using UnityEngine;

public class PlacementSystem : MonoBehaviour {
    [SerializeField] private GameObject cellIndicator;
    [SerializeField] private InputManager inputManager;
    [SerializeField] private Grid grid;

    [SerializeField] private TorretsData torretsData;
    private int selectedTorretsIndex = -1;
    [SerializeField] private GameObject gridVisualization;

    private GridData furnitureData;
    private Renderer previewRenderer;

    private List<GameObject> placedGameObjects = new();
    [SerializeField] private List<Vector3Int> tilesToBlock;
    private Vector3Int lastDetectedPosition = Vector3Int.zero;

    private void Start() {
        StopPlacement();
        
        furnitureData = new GridData();

        previewRenderer = cellIndicator.GetComponentInChildren<Renderer>();
        foreach (var tilePosition in tilesToBlock) {
            BlockTile(tilePosition);
        }
    }

    public void StartPlacement(int ID) {

        selectedTorretsIndex = torretsData.torrentData.FindIndex(data => data.ID == ID);
        if (selectedTorretsIndex == -1) return;

        gridVisualization.SetActive(true);
        cellIndicator.SetActive(true);
        inputManager.OnClicked += PlaceTorrets;
        inputManager.OnExit += StopPlacement;
    }

    private void PlaceTorrets() {
        if (selectedTorretsIndex == -1) return;

        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        if (!CheckPlacementValidity(gridPosition, selectedTorretsIndex)) return;

        GameObject newObject = Instantiate(torretsData.torrentData[selectedTorretsIndex].Prefab);
        newObject.transform.position = grid.CellToWorld(gridPosition);
        placedGameObjects.Add(newObject);

        furnitureData.AddObjectAt(
            gridPosition,
            torretsData.torrentData[selectedTorretsIndex].Size,
            torretsData.torrentData[selectedTorretsIndex].ID,
            placedGameObjects.Count - 1
        );
    }

    private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedTorretsIndex) {
        return furnitureData.CanPlaceObjectAt(gridPosition, torretsData.torrentData[selectedTorretsIndex].Size);
    }

    public void BlockTile(Vector3Int gridPosition) {
        furnitureData.BlockTile(gridPosition);
    }

    private void StopPlacement() {
        if (selectedTorretsIndex == -1) return;

        selectedTorretsIndex = -1;
        gridVisualization.SetActive(false);
        cellIndicator.SetActive(false);
        inputManager.OnClicked -= PlaceTorrets;
        inputManager.OnExit -= StopPlacement;
    }

    private void Update() {
        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        bool placementValidity = (selectedTorretsIndex != -1) && CheckPlacementValidity(gridPosition, selectedTorretsIndex);
        previewRenderer.material.color = placementValidity ? Color.white : Color.red;

        cellIndicator.transform.position = grid.CellToWorld(gridPosition);

        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            StartPlacement(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            StartPlacement(2);
        }
        if (lastDetectedPosition != gridPosition) {
            lastDetectedPosition = gridPosition;
        }

        //if (Input.GetMouseButtonDown(1)) {
        //    tilesToBlock.Add(lastDetectedPosition);
        //}
    }
}
