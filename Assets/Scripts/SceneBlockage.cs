using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneBlockage : MonoBehaviour
{
    public int associatedStageIndex = 0;
    
    private SceneGoalManager.StageAdvanceDelegate blockageActiveFunction;
    private void Start()
    {
        blockageActiveFunction = ((int newStageIndex) => { SetBlockageActiveState(newStageIndex != associatedStageIndex); });
        SceneGoalManager.goalManager.OnStageAdvance += blockageActiveFunction;
    }

    private void OnDestroy()
    {
        SceneGoalManager.goalManager.OnStageAdvance -= blockageActiveFunction;
    }
    public void SetBlockageActiveState(bool newState)
    {
        GetComponent<BoxCollider2D>().enabled = newState;
    }
}
