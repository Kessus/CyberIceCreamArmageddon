using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneBlockage : MonoBehaviour
{
    public int associatedStageIndex = 0;
    private void Start()
    {
        SceneGoalManager.goalManager.OnStageAdvance += ((int newStageIndex) => { SetBlockageActiveState(newStageIndex != associatedStageIndex); });
    }

    private void OnDestroy()
    {
        
    }
    public void SetBlockageActiveState(bool newState)
    {
        GetComponent<BoxCollider2D>().enabled = newState;
    }
}
