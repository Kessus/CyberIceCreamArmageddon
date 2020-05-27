using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//When crossed, starts the next stage in the SceneGoalManager and re-enables the given scene blockage; only works once
public class NextStageTrigger : MonoBehaviour
{
    public SceneBlockage controlledBlockage;

    private bool wasTriggered = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (wasTriggered || collision.gameObject.layer != LayerMask.NameToLayer("Player"))
            return;

        SceneGoalManager.goalManager.BeginNewStage();
        controlledBlockage.SetBlockageActiveState(true);
        wasTriggered = true;
    }
}
