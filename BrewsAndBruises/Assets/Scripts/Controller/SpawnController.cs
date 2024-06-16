using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour{
    [SerializeField]
    public GameObject spawnObj;

    [SerializeField]
    public float range = 5.0f; 

    [SerializeField]
    public float enemySwarmInterval = 3.5f;

    void Start()
    {
        range = Mathf.Abs(range);
        StartCoroutine(spawnObject(enemySwarmInterval, spawnObj));
    }

    private IEnumerator spawnObject(float interval, GameObject toSpawnedObject) {
        yield return new WaitForSeconds(interval);
        GameObject newObj = Instantiate(
            toSpawnedObject, 
            new Vector3(gameObject.transform.position.x + Random.Range(-1*range,range),0,gameObject.transform.position.z + Random.Range(-1*range,range)),
            Quaternion.identity
        );
        StartCoroutine(spawnObject(interval, toSpawnedObject));
    }
}
