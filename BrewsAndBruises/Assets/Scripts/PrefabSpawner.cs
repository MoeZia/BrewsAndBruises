using UnityEngine;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class PrefabSpawner : MonoBehaviour
{
    public List<GameObject> prefabsToSpawn;
    public List<GameObject> Spawnpoints;
    public int prefabIndex = 0; // Index to select which prefab to spawn
    public Vector3 spawnPoint = new Vector3(0, 0, 0);
    public int gridDimension = 9;

    public void SpawnSelectedPrefab()
    {
        if (prefabsToSpawn.Count > 0 && prefabIndex >= 0 && prefabIndex < prefabsToSpawn.Count)
        {
            GameObject prefab = prefabsToSpawn[prefabIndex];
            SpawnGridAtPoint(gridDimension, spawnPoint, prefab);
        }
        else
        {
            Debug.LogError("Prefab index out of range or no prefabs assigned.");
        }
    }

    public void SpawnGridAtPoint(int dimension, Vector3 specifiedSpawnPoint, GameObject prefab)
    {
        float offset = 1.0f; // Spacing between prefabs
        Vector3 startPosition = specifiedSpawnPoint - new Vector3(dimension / 2.0f * offset, 0, dimension / 2.0f * offset);

        for (int i = 0; i < dimension; i++)
        {
            for (int j = 0; j < dimension; j++)
            {
                Vector3 position = startPosition + new Vector3(i * offset, 0, j * offset);
                Instantiate(prefab, position, Quaternion.identity);
            }
        }
    }

    // Public function to allow external control over prefab spawning
    public void SpawnPrefabGridFromIndex(int index, int dimension, Vector3 spawnPt)
    {
        if (index >= 0 && index < prefabsToSpawn.Count)
        {
            GameObject selectedPrefab = prefabsToSpawn[index];
            SpawnGridAtPoint(dimension, spawnPt, selectedPrefab);
        }
        else
        {
            Debug.LogError("Prefab index out of range.");
        }
    }
    public void SpawnPrefabGridFromIndexwithSpawnPointIndex(int index, int dimension, int sPt){
        if (index >= 0 && index < prefabsToSpawn.Count)
        {
            GameObject selectedPrefab = prefabsToSpawn[index];
            Vector3 spawnPt = Spawnpoints[sPt].transform.position;
            SpawnGridAtPoint(dimension, spawnPt, selectedPrefab);
        }
        else
        {
            Debug.LogError("Prefab index out of range.");
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(PrefabSpawner))]
    public class PrefabSpawnerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector(); // Draws all the default items

            PrefabSpawner spawner = (PrefabSpawner)target;
            if (GUILayout.Button("Spawn Prefab"))
            {
                spawner.SpawnSelectedPrefab();
            }
        }
    }
#endif
}
