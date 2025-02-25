using System;
using System.Collections.Generic;
using UnityEngine;

public class GridData {
    Dictionary<Vector3Int, PlacementData> placedObjects = new();
    HashSet<Vector3Int> blockedTiles = new();

    public void AddObjectAt(Vector3Int gridPosition,
                            Vector2Int objectSize,
                            int ID,
                            int placedObjectIndex) {
        List<Vector3Int> positionToOccupy = CalculatePositions(gridPosition, objectSize);
        PlacementData data = new PlacementData(positionToOccupy, ID, placedObjectIndex);

        foreach (var pos in positionToOccupy) {
            if (placedObjects.ContainsKey(pos)) {
                throw new Exception($"Dictionary already contains this cell position {pos}");
            }
            placedObjects[pos] = data;
        }
    }

    private List<Vector3Int> CalculatePositions(Vector3Int gridPosition, Vector2Int objectSize) {
        List<Vector3Int> returnVal = new();
        for (int x = 0; x < objectSize.x; x++) {
            for (int y = 0; y < objectSize.y; y++) {
                returnVal.Add(gridPosition + new Vector3Int(x, 0, y));
            }
        }
        return returnVal;
    }
    public bool CanPlaceObjectAt(Vector3Int gridPosition, Vector2Int objectSize) {
        List<Vector3Int> positionToOccupy = CalculatePositions(gridPosition, objectSize);
        foreach (var pos in positionToOccupy) {
            if (placedObjects.ContainsKey(pos) || blockedTiles.Contains(pos)) {
                return false;
            }
        }
        return true;
    }

    internal int GetRepresentationIndex(Vector3Int gridPosition) {
        return placedObjects.TryGetValue(gridPosition, out var data) ? data.PlacedObjectIndex : -1;
    }

    internal void RemoveObjectAt(Vector3Int gridPosition) {
        if (!placedObjects.ContainsKey(gridPosition)) return;

        foreach (var pos in placedObjects[gridPosition].occupiedPositions) {
            placedObjects.Remove(pos);
        }
    }

    public void BlockTile(Vector3Int gridPosition) {
        blockedTiles.Add(gridPosition);
    }

    public void UnblockTile(Vector3Int gridPosition) {
        blockedTiles.Remove(gridPosition);
    }
    public void ClearBlockedTiles() {
        blockedTiles.Clear();
    }

}


public class PlacementData {
    public List<Vector3Int> occupiedPositions;
    public int ID { get; private set; }
    public int PlacedObjectIndex { get; private set; }

    public PlacementData(List<Vector3Int> occupiedPositions, int iD, int placedObjectIndex) {
        this.occupiedPositions = occupiedPositions;
        ID = iD;
        PlacedObjectIndex = placedObjectIndex;
    }
}
