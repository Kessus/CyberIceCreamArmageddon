using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

//Manages all of the enemy spawners and coordinates their work
public class SpawnerManager : MonoBehaviour
{
    public static SpawnerManager mainManager;
    //Desired amount of enemies present on the stage at once
    public int desiredEnemyCount = 4;
    public float initialSpawnDelay = 4.0f;
    public float spawnFrequency = 3.0f;
    //Spawn frequency varies based on this value
    public float spawnFrequencyMaxOffset = 0.0f;

    private List<Spawner> spawners;
    private int activeEnemies = 0;

    private void Awake()
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
        //Gathers only non-visible spawners corresponding to the currently active stage
        List<Spawner> availableSpawners = spawners.Where(spawner => spawner.IsReady && spawner.associatedStage == SceneGoalManager.goalManager.CurrentStageIndex).ToList();
        int enemyCount = activeEnemies;
        //Spawns only the amount of enemies that is allowed at a given time which is either the desired maximum or the number of enemies left to complete the current stage
        for (int i = 0; i < Mathf.Min(desiredEnemyCount - enemyCount, SceneGoalManager.goalManager.RemainingEnemies - enemyCount); i++)
        {
            if (availableSpawners.Count > 0)
            {
                int spawnerIndex = Random.Range(0, availableSpawners.Count);
                availableSpawners[spawnerIndex].Spawn();
                availableSpawners.RemoveAt(spawnerIndex); //One spawner can only spawn one enemy during one trigger
            }
            else
                break;
        }

        //Randomizes and applies the spawner trigger cooldown each time, then triggers the coroutine again
        float cooldownTime = Random.Range(-spawnFrequencyMaxOffset, spawnFrequencyMaxOffset) + spawnFrequency;
        cooldownTime = cooldownTime < 0.0f ? 0.0f : cooldownTime;
        yield return new WaitForSeconds(cooldownTime);
        StartCoroutine(TriggerSpawners());
    }
}
