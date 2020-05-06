using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public List<GameObject> spawnedEnemyPrefabs;
    public bool IsReady { get; private set; } = true;

    public void Spawn()
    {
        if (spawnedEnemyPrefabs.Count > 0)
            Instantiate(spawnedEnemyPrefabs[Random.Range(0, spawnedEnemyPrefabs.Count)], transform.position, transform.rotation);
        else
            Debug.LogError("Spawner has no assigned enemy prefabs: " + transform.name);
    }

    private void Start()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, 0.0f);
        SpawnerManager.mainManager.RegisterSpawner(this);
    }

    private void OnBecameInvisible()
    {
        IsReady = true;
    }

    private void OnBecameVisible()
    {
        IsReady = false;
    }
}
