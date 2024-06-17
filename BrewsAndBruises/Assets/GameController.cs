using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    PrefabSpawner prefabSpawner;
    float x = 20;
    float z = 170;
    int prefabint = 0;
    int gridDimension = 9;

    // Start is called before the first frame update
    void Start()
    {

        prefabSpawner = GetComponent<PrefabSpawner>();
        Vector3 spawnPoint = new Vector3(x, 0, z);
        prefabSpawner.SpawnPrefabGridFromIndex(prefabint, gridDimension, spawnPoint);
        prefabSpawner.SpawnPrefabGridFromIndex(1, 3, spawnPoint);
        // adding 4 Hords at different locations at start of the game ----> later trigger zones to take of load !
        spawnPoint = new Vector3(x+ 50, 0, z );
        prefabSpawner.SpawnPrefabGridFromIndex(prefabint, gridDimension, spawnPoint);
        prefabSpawner.SpawnPrefabGridFromIndex(1, 3, spawnPoint);

        spawnPoint = new Vector3(x+ 100, 0, z );
        prefabSpawner.SpawnPrefabGridFromIndex(prefabint, gridDimension, spawnPoint);
        prefabSpawner.SpawnPrefabGridFromIndex(1, 3, spawnPoint);

        spawnPoint = new Vector3(x+ 150, 0, z );
        prefabSpawner.SpawnPrefabGridFromIndex(prefabint, gridDimension, spawnPoint);
        prefabSpawner.SpawnPrefabGridFromIndex(1, 3, spawnPoint);

        spawnPoint = new Vector3(x+ 200, 0, z );
        prefabSpawner.SpawnPrefabGridFromIndex(prefabint, gridDimension, spawnPoint);
        prefabSpawner.SpawnPrefabGridFromIndex(1, 3, spawnPoint);



        
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
