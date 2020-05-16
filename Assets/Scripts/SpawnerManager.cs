using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SpawnerManager : MonoBehaviour
{
    public static SpawnerManager mainManager;
    public int desiredEnemyCount = 4;
    public float initialSpawnDelay = 4.0f;
    public float spawnFrequency = 3.0f;
    public float spawnFrequencyMaxOffset = 0.0f;

    private List<Spawner> spawners;
    private int activeEnemies = 0;

    public SpawnerManager()
    {
        mainManager = this;
        spawners = new List<Spawner>();
    }

    void Start()
    {
        StartCoroutine(InitializeSpawning());
    }

    private IEnumerator InitializeSpawning()
    {
        yield return new WaitForSeconds(initialSpawnDelay);
        StartCoroutine(TriggerSpawners());
    }

    public void RegisterSpawner(Spawner spawner)
    {
        if (!spawners.Contains(spawner))
            spawners.Add(spawner);
    }

    public void RegisterEnemySpawn()
    {
        activeEnemies++;
    }

    public void RegisterEnemyDeath()
    {
        activeEnemies--;
    }

    private IEnumerator TriggerSpawners()
    {
        List<Spawner> availableSpawners = spawners.Where(spawner => spawner.IsReady && spawner.associatedStage == SceneGoalManager.goalManager.CurrentStageIndex).ToList();
        int enemyCount = activeEnemies;
        for (int i = 0; i < Mathf.Min(desiredEnemyCount, SceneGoalManager.goalManager.RemainingEnemies - enemyCount); i++)
        {
            if (availableSpawners.Count > 0)
            {
                int spawnerIndex = Random.Range(0, availableSpawners.Count);
                availableSpawners[spawnerIndex].Spawn();
                availableSpawners.RemoveAt(spawnerIndex);
            }
            else
                break;
        }

        float cooldownTime = Random.Range(-spawnFrequencyMaxOffset, spawnFrequencyMaxOffset) + spawnFrequency;
        cooldownTime = cooldownTime < 0.0f ? 0.0f : cooldownTime;
        yield return new WaitForSeconds(cooldownTime);
        StartCoroutine(TriggerSpawners());
    }
}
