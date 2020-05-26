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
    public GameObject stageProgressionArrow;
    public GameObject stageCompletionScreen;
    public int CurrentStageIndex { get; private set; } = -1;
    public int RemainingEnemies { get; private set; } = 0;
    public float StageDuration { get; private set; } = 0.0f;

    private CameraFollow cameraScript;
    public bool StageComplete { get; private set; } = false;

    private void Awake()
    {
        goalManager = this;
    }
    void Start()
    {
        cameraScript = Camera.main.GetComponent<CameraFollow>();
        CurrentStageIndex = 0;
        RemainingEnemies = stageEnemyCountGoals[0];
        StageComplete = false;
        StageDuration = 0.0f;
        OnStageAdvance?.Invoke(CurrentStageIndex);
        BeginNewStage();
    }

    private void Update()
    {
        if(!StageComplete)
            StageDuration += Time.deltaTime;
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
    public void AdvanceToNextStage()
    {
        CurrentStageIndex++;
        if (stageEnemyCountGoals.Count > CurrentStageIndex)
        {
            OnStageAdvance?.Invoke(CurrentStageIndex);
            stageProgressionArrow.SetActive(true);
        }
        else
        {
            StageComplete = true;
            stageCompletionScreen.SetActive(true);
        }
    }

    //Start spawning new enemies
    public void BeginNewStage()
    {
        RemainingEnemies = stageEnemyCountGoals[CurrentStageIndex];
        enemiesLeftText.text = "Enemies left: " + RemainingEnemies;
        stageProgressionArrow.SetActive(false);
    }
}
