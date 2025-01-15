using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TorretsSO", menuName = "ScriptableObjects/Torrets", order = 1)]

public class TorretsData : ScriptableObject {
    public List<TorrentData> torrentData;
}

[Serializable]
public class TorrentData {

    public string Name;
    public int ID;
    public Vector2Int Size= Vector2Int.one;
    public GameObject Prefab;
}

