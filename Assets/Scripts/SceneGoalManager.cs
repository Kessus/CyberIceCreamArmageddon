using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneGoalManager : MonoBehaviour
{
    public static SceneGoalManager goalManager;
    public List<int> stageEnemyCountGoals;
    public delegate void StageAdvanceDelegate(int newStageIndex);
    public event StageAdvanceDelegate OnStageAdvance;
    public Text enemiesLeftText;
    public int CurrentStageIndex { get; private set; } = -1;
    public int RemainingEnemies { get; private set; } = 0;

    private CameraFollow cameraScript;    
    
    public SceneGoalManager()
    {
        goalManager = this;
    }
    void Start()
    {
        cameraScript = Camera.main.GetComponent<CameraFollow>();
        CurrentStageIndex = 0;
        OnStageAdvance?.Invoke(CurrentStageIndex);
        BeginNewStage();
    }

    public void RegisterEnemyDeath()
    {
        RemainingEnemies--;
        if(enemiesLeftText != null)
            enemiesLeftText.text = "Enemies left: " + RemainingEnemies;
        if (RemainingEnemies <= 0)
        {
            AdvanceToNextStage();
        }
    }
    
    //Allow the player to progress further without actually spawning new enemies
    private void AdvanceToNextStage()
    {
        CurrentStageIndex++;
        OnStageAdvance?.Invoke(CurrentStageIndex);
    }

    //Start spawning new enemies
    public void BeginNewStage()
    {
        RemainingEnemies = stageEnemyCountGoals[CurrentStageIndex];
        enemiesLeftText.text = "Enemies left: " + RemainingEnemies;
    }
}
