using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//A helper object designed to spawn enemies at a specified place while randomizing spawned enemy type
public class Spawner : MonoBehaviour
{
    public List<GameObject> spawnedEnemyPrefabs;
    public int associatedStage = 0;
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
        //Makes sure that the spawner is placed at position z = 0
        transform.position = new Vector3(transform.position.x, transform.position.y, 0.0f);
        SpawnerManager.mainManager.RegisterSpawner(this);
    }

    //Spawners are only able to be active while not being seen by the player
    private void OnBecameInvisible()
    {
        IsReady = true;
    }

    private void OnBecameVisible()
    {
        IsReady = false;
    }
}
