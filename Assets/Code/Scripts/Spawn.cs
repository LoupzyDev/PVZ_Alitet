using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour {
    [SerializeField] private List<Vector3> spawns;
    [SerializeField] private float spawnTime;          
    [SerializeField] private GameObject spaceShipPrefab; 

    private void Start() {
        StartCoroutine(SpawnShip());
    }

    private IEnumerator SpawnShip() {
        while (true)  
        {
            yield return new WaitForSeconds(spawnTime);

            int randomIndex = Random.Range(0, spawns.Count);
            Instantiate(spaceShipPrefab, spawns[randomIndex], Quaternion.identity);
        }
    }
}
